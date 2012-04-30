# WebActivator 
ASP.NET Web Application Startup Code Made Easy

## Intro
Many third party libraries require more than a simple assembly reference to be useful. They sometimes need the application to run a bit of configuration code when the application starts up. In the past, this meant you would need to copy and paste a bit of startup code in the `Application_Start` method of `Global.asax`. WebActivator, is a library (delivered as a NuGet package) that solves that issue when combined with NuGet’s ability to include source code files in packages along with referenced assemblies. It makes it easy for a NuGet package to include a bit of application start-up code.

## More Reading
Be sure to read David Ebbo’s blog post on the subject.
[Light up your NuGets with startup code and WebActivator](http://blogs.msdn.com/b/davidebb/archive/2010/10/11/light-up-your-nupacks-with-startup-code-and-webactivator.aspx)