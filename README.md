# CSVExtensions

Extension methods for exporting CSV files from `IEnumerable<T>`, and a `CSVFileResult` for ASP.NET

## Basic Usage

With any IEnumerable\<T\> call `ToCSV([withHeader = true])` to get a properly formatted (RFC 4180 compliant) 
and escaped `IEnumerable<string>`, with each item being a CSV row.

```
List<Product> products = repository.GetProductList();

var csv = products.ToCSV();

```
It can be used in the same way against an `IEnumerable<dynamic>`

```
List<dynamic> products = repository.GetProductList();

var csv = products.ToCSV();

```

By default **ToCSV** will add property names as the first row, disable this by passing false 
to the **withHeader** argument. `products.toCSV(false);`

## ASP.NET

With ASP.NET you can pass your `IEnumerable<T>` to a new `CSVFileResult` and return this, 
you don't need to call ToCSV() yourself.

```
public ActionResult DownloadCsv()
{
    var result = repository.GetSomeData();

    return new CsvFileResult<dynamic>(result, "fileName.csv");
}
```

## Additional methods

`ToCsvRow(), ToCSVRow(PropertyInfo[] properties = null)`

Produces a valid, escaped CSV row from an object. You can pass in an array of `PropertyInfo` objects
if you have them to prevent the method using reflection to get them each time you call it.

`string.EscapeForCsv()`

An extension method that escapes, then returns the string for use as a CSV value (based on RFC 4180).
