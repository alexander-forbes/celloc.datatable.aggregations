[![Build Status](https://travis-ci.org/sduplooy/Celloc.DataTable.Aggregations.svg?branch=master)](https://travis-ci.org/sduplooy/Celloc.DataTable.Aggregations)
[![NuGet Badge](https://buildstats.info/nuget/Celloc.DataTable.Aggregations)](https://www.nuget.org/packages/Celloc.DataTable.Aggregations/)

# Celloc.DataTable.Aggregations

![Logo](https://raw.githubusercontent.com/wiki/sduplooy/Celloc/images/186401-64-plugin.png)

An aggregations extension for Celloc.DataTable.

## NuGet
To install the package from NuGet, run the following command:

`Install-Package Celloc.DataTable.Aggregations`

## Usage

### GroupBy
To group data rows by a specific column, use the `GroupBy` method:

```C#
myDataTable.GroupBy("A1:A?");
```
or
```C#
myDataTable.GroupBy(((0,0),(0,5));
```

If the specified range does not exist in the the data table a `null` will be returned. If the range spans more than one column an `ArgumentException` will be thrown. 

### Sum

To sum a column, use the `Sum` method:

```C#
myDataTable.Sum<int>("A1:A?");
```
or
```C#
myDataTable.Sum<int>(((0,0),(0,5));
```
