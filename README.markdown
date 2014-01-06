WebActivator
============

WebActivator is a [NuGet](http://nuget.org/) package that allows other packages to easily bring in Startup and Shutdown code into a web application. This gives a much cleaner solution than having to modify global.asax with the startup logic from many packages.

## Integrating WebActivator into your package

### Step 1: Add a dependency on it in your package's nuspec file

	<?xml version="1.0"?>
	<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd">
	  <metadata>
	    <id>MySuperPackage</id>
	    etc...
	    <dependencies>
	      <dependency id="WebActivatorEx" version="2.0.0" />
	    </dependencies>
	  </metadata>
	</package>

The only thing that needs to be added for WebActivator is the `<dependency>` line.

### Step 2: Add a preprocessed source file in Content\App_Start

Under the folder where your nuspec file is, create a Content\App_Start folder. In there, create a file named MySuperPackage.cs.pp:

	using System;

	[assembly: WebActivatorEx.PreApplicationStartMethod(
	    typeof($rootnamespace$.App_Start.MySuperPackage), "PreStart")]

	namespace $rootnamespace$.App_Start {
	    public static class MySuperPackage {
	        public static void PreStart() {
	            // Add your start logic here
	        }
	    }
	}

And that's it! Now your code will be called early on when the app starts.

## Choosing when your code executes

The example above uses the PreApplicationStartMethod attribute, which makes the code run very early on, before global.asax's Application_Start gets to run.

If that's too early for you, you can instead use the PostApplicationStartMethod attribute, which gets your code called //after// global.asax's Application_Start.

And if you have some logic you'd like to execute when the app shuts down, you can use the ApplicationShutdownMethod attribute.

Other than their name changing, all three attributes are used the same way.

## Please follow the folder convention

As you saw in the example above, I placed the .cs.pp file under the App_Start folder, such that when someone installs your package, the .cs file ends up in the App_Start folder under the app root. Technically, WebActivator does not require this, but it's a good convention to follow so that the startup code from all the packages all ends up in one folder rather than being at the root of the app (which we don't want to pollute).

The picky ones will bring up that App_Start is not a great name when you use WebActivator for shut down logic. Maybe so, but since shut down logic is somewhat rare, and often goes along with startup logic, it's best to keep everything under App_Start and live with this small naming anomaly :)

## Support for Web Sites

In a Web Site (as opposed to a Web Application), you typically put your shared code in the App_Code folder. If you have code in there that uses the PostApplicationStartMethod attribute, it will get called when the app starts, giving Web Sites some WebActivator love.

Please note that you can only use PostApplicationStartMethod in App_Code, and not PreApplicationStartMethod. The reason is that when PreApplicationStartMethod fires, the App_Code folder has not even been compiled!

## Support for invoking the start methods outside of ASP.NET

This change came courtesy of [Jakub Konecki](http://stackoverflow.com/users/449906/jakub-konecki), who needed it for unit testing purpose. This comes as a set of static methods that you can use to invoke the startup methods:

	// Run all the WebActivator PreStart methods
	WebActivator.ActivationManager.RunPreStartMethods();

	// Run all the WebActivator PostStart methods
	WebActivator.ActivationManager.RunPostStartMethods();

	// Run all the WebActivator start methods
	WebActivator.ActivationManager.Run();

	// Run all the WebActivator shutdown methods
	WebActivator.ActivationManager.RunShutdownMethods();

Note that normally you would not call these methods explicitly from a web application. But if you are using some 'WebActivated' NuGet packages from a different type of apps (say a Console app), it could make sense to have add calls to Run() and RunShutdownMethods() at the beginning and end of your app.

## Change history

### 2.0.4 (11/18/2013)

* Fixed to work on Mono https://github.com/davidebbo/WebActivator/pull/15

### 2.0.3 (8/6/2013)

* Make the Order parameter work across multiple assemblies https://github.com/davidebbo/WebActivator/pull/13

### 2.0.2 (6/19/2013)

* Ignore GetCustomAttributes exceptions to work around https://github.com/davidebbo/WebActivator/issues/12

### 2.0.1 (2/9/2013)

* The WebActivator assembly is now signed (and is now named WebActivatorEx.dll)

### 1.5.3 (12/20/2012)

Change contributed by @gdoten

* Look in CodeBase rather than Location directory for assemblies to scan for attirbutes.
* Make Assembly.LoadFrom exception handling a little more specific.
* Fix to unit testing ExecuteOrder when all test are run at the same time.

### 1.5.2 (11/2/2012)

* Only difference with 1.5.1 is the corrected project site link so it points to github instead of the old bitbuck

### 1.5.1 (5/31/2012)

* Rerelease after moving from bitbucket to github

### 1.5 (10/26/2011)

* Added ability to order method invocations within one assembly

### 1.4.3 (9/14/2011)

* Take dependency on Microsoft.Web.Infrastructure

### 1.4 (2/28/2011)

* Added ability to run code when the app shuts down

### 1.3 (2/24/2011)

* Added support for Web Sites

### 1.2 (2/21/2011)

* Added support for executing WebActivator outside ASP.NET runtime

### 1.1 (2/15/2011)

* Added support for methods called after global.asax App_Start

### 1.0 (10/8/2010)

* Initial release


## Related blog posts

[Light up your NuGets with startup code and WebActivator](http://blogs.msdn.com/b/davidebb/archive/2010/10/11/light-up-your-nupacks-with-startup-code-and-webactivator.aspx)

[New features in WebActivator 1.4](http://blog.davidebbo.com/2011/02/new-features-in-webactivator-13.html)

[App_Start folder convention for NuGet and WebActivator](http://blog.davidebbo.com/2011/02/appstart-folder-convention-for-nuget.html)

