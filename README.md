# unit4-automation

[![Travis CI](https://travis-ci.org/tomcsmith1990/unit4-automation.svg?branch=master)](https://travis-ci.org/tomcsmith1990/unit4-automation)

This provides automation for [Unit4's Business World On!](https://www.unit4.com/uki/applications/erp/business-world) (formerly known as Agresso).

It makes SOAP requests using Unit4's _Resql_ langauge. It pulls together many of these requests to form the overall output, since larger requests timeout most of the time.

It uses the [commandlineparser](https://github.com/commandlineparser/commandline/wiki) library.

## Available Commands

```
bcr        Produce a BCR.

config     Configure the Unit4 connection details.

help       Display more information on a specific command.

version    Display version information.
```

### Config Command

This sets up the connection to Unit4 Business World.

Options:
```
--client     Set the Unit4 client.

--url        Set the Unit4 SOAP service URL.

--help       Display this help screen.

--version    Display version information.
```

### BCR Command

The `bcr` command produces an Excel spreadsheet with a bcr which matches the filters.

Must use the `config` command first to set the client and url.
Must add `Unit4 Automation` credentials in Credential Manager.

Options:
```
--tier1         Filter by a tier 1 code.

--tier2         Filter by a tier 2 code.

--tier3         Filter by a tier 3 code.

--tier4         Filter by a tier 4 code.

--costcentre    Filter by a cost centre.

--output        The directory to save the bcr in. The directory must already exist.
```