function scriptingBridge(options) {

    function invoke(method, options, json) {

        var result;
        var args;
        var namespace;
        var context;
        var func;
        var jsonResult;

        context = window;
        namespace = method.split('.');
        func = namespace.pop();

        // Resolve the context
        for (var i = 0; i < namespace.length; i++) {
            context = context[namespace[i]];
        }

        // Prepare arguments and invoke target function.
		if(json)
			args = JSON.parse(json);
		else
			args = undefined;
        result = context[func].apply(this, args);

        // Serialize and return result
        if (options == 'json' && result) {
            try {
                jsonResult = JSON.stringify(result);
            } catch (err) { }
            if (!jsonResult)
                jsonResult = result.toString();
            return jsonResult;
        }
        return result;
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
        documentMode: window.document.documentMode
    };

    // Register the call gate
    if (window.external && typeof window.external.RegisterClient != "undefined") {
        window.external.RegisterClient(JSON.stringify(configuration));
    }
}
