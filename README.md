# Health Application for Web Applications

## Build Dependencies

* NuGet
* .NET Framework 4.5.2 - Full
* MSBuild 14

## Usage

```sh
./Health.exe -Url https://localhost/ -Interval 30 -GracePeriod 240
```

Arguments:

* -Uri is a required argument and is the URL to check against
* -Interval is not required, is the time between tests, and defaults to 30 seconds
* -GracePeriod is optional, and is the amount of time where health chack failures will be ignored
* -Timeout is optional, and is the max wait time for a health check to respond

The health application will quit if it does not get a sucess code from the web server.

## Note


There is a default 10 second timeout for each HTTP/S request. This means that the specified page must respond within 10 seconds.
