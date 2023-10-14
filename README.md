WebBrowser Tools
=================

[![Nuget version](https://img.shields.io/nuget/v/Vereyon.Windows.WebBrowser)](https://www.nuget.org/packages/Vereyon.Windows.WebBrowser/)

Vereyon's Windows.WebBrowser is a helper library for controlling and enhancing the `System.Windows.Forms.WebBrowser` control which embeds Microsoft Internet Explorer. It improves JavaScript interoperability and enables controlling the IE version and GPU acceleration.

Documentation
-------------

### Scripting Bridge

The `ScriptingBridge` is a utility which makes it possible to invoke JavaScript member functions in the embedded browser and to receive return values and objects. This is in contrast to the `HtmlDocument.InvokeScript()` method which can only invoke global functions. It also provides enhanced error handling by for example differentiating between a not found function and a function returning void.

#### Usage

If you for example want to invoke `myObject.myFunction` as defined in the following JavaScript snippet:

```
var myObject = {
	memberFunction: function()
	{
		return "some result";
	}
};
```

You can do so using the `ScriptingBridge` as follows where `webBrowser` is your `System.Windows.Forms.WebBrowser` instance:

```
var bridge = new ScriptingBridge(webBrowser, true);
var result = bridge.InvokeFunction<string>("myObject.myFunction", parameter);
```

### Internet Feature Control

The `InternetFeatureControl` is a utility class which can be used to control features of the `System.Windows.Forms.WebBrowser` control.

#### Browser emulation

Using `InternetFeatureControl.SetBrowserEmulation()` the Internet Explorer rendering engine version can be controlled.

#### GPU acceleration

Using `InternetFeatureControl.SetGpuRendering()` GPU acceleration for rendering can be enabled and disabled.


### WinInet Cache Control

The `WinInetCacheControl` is a utility class which can be used to controle the WinInet cache.

Using `InternetFeatureControl.ClearCache()` the WinInet cache can be fully cleared.

Unit tests
----------

See the Windows.WebBrowser.Tests project for xUnit based unit tests.


More information
----------------

 * [Internet Feature Control Keys on MSDN](https://msdn.microsoft.com/en-us/library/ee330720%28v=vs.85%29.aspx)
 * [Controlling WebBrowser Control Compatibility on MSDN Blog](http://blogs.msdn.com/b/patricka/archive/2015/01/12/controlling-webbrowser-control-compatibility.aspx)
 * [Web Browser Control – Specifying the IE Version](https://weblog.west-wind.com/posts/2011/May/21/Web-Browser-Control-Specifying-the-IE-Version)

License
-------

[MIT X11](http://en.wikipedia.org/wiki/MIT_License)
