WebBrowser Tools
=================

Vereyon's Windows.WebBrowser is a helper library for controlling and enhancing the `System.Windows.Forms.WebBrowser` control which embeds Microsoft Internet Explorer.

Documentation
-------------

### Scripting Bridge

The `ScriptingBridge` is a utility which makes it possible to invoke JavaScript member functions in the embedded browser and to receive return values and objects. This is in contrast to the `HtmlDocument.InvokeScript()` method which can only invoke global functions.

```
var bridge = new ScriptingBridge(webBrowser, true);
var result = Bridge.InvokeFunction<ScriptingReturnData>("myObject.myFunction", parameter);
```

### Internet Feature Control

The `InternetFeatureControl` is a utility class which can be used to control features of the `System.Windows.Forms.WebBrowser` control.

#### Browser emulation

Using `InternetFeatureControl.SetBrowserEmulation()` the Internet Explorer rendering engine version can be controlled.

#### GPU acceleration

Using `InternetFeatureControl.SetGpuRendering()` GPU acceleration for rendering can be enabled and disabled.

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
