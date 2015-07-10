function scriptingBridge(options) {

    function invoke(method, json) {

        var result;
        var args;

		var result = {
			success: true,
			result: null,
			error: null
		};

		// The root context is assumed to be the window object. The last part of the method parameter is the actual function name.
        var context = window;
        var namespace = method.split('.');
        var func = namespace.pop();

        // Resolve the context
        for (var i = 0; i < namespace.length; i++) {
            context = context[namespace[i]];

			// Check if the context is defined so far.
			if(context == undefined)
			{
				return JSON.stringify({
					success: false,
					error: namespace.slice(0, i + 1).join('.') + ' is undefined.'
				});
			}
        }

		// Check the target function.
		if(context[func] == undefined)
		{
			return JSON.stringify({
				success: false,
				error: method + ' is undefined.'
			})
		}
		if(typeof(context[func]) != 'function')
		{
			return JSON.stringify({
				success: false,
				error: method + ' is not a function.'
			});
		}

        // Prepare arguments and invoke target function.
		args = JSON.parse(json);
        result = context[func].apply(context, args);

		// Serialize the result and wrap the function result in a result object.
		// Do not directly serialize the complete result object as the user may use different
		// serialization options.
		jsonResult = JSON.stringify(result);
		result = {
			result: jsonResult,
			success: true
		};

		// Serialize and return result object.
		return JSON.stringify(result);
    }

    var callGateName;
    var self = this;

    // Define the call gate in the window scope to make it accessable to the .NET world.
    callGateName = "__scriptingBridge__callGate";
    window[callGateName] = function () {
        return invoke.apply(self, arguments);
    }

    // Prepare registration configuration.
    var configuration = {
        callGatename: callGateName,
        documentMode: window.document.documentMode,
		jsonSupported: (JSON != undefined && JSON.stringify != undefined && JSON.parse != undefined)
    };

    // Register the call gate
    if (window.external && typeof window.external.RegisterClient != "undefined") {
        window.external.RegisterClient(JSON.stringify(configuration));
    }
}
