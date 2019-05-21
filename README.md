Attribute Sniffer
==================

Attribute Sniffer is a tool that extracts code attribute metrics from C# source code. It derives from the [Annotation Sniffer](https://github.com/phillima/asniffer). 


### How to use

```
dotnet run --project AttributeSniffer.csproj <path to project> <path to report> <single/multi> <report type>
```

The path must be a root folder that contains other projects directories, for the "multi" case. 
Follow the directory arrangement below.

    .
    ├── projects                # Root directory for projects. This is the path to be provided
        ├── project1            # Contains the source files for project1
        ├── project2            # Contains the source files for project2
        └── ...         

If the option "single" is used, then the path provided references only a single project, with all the source files inside that folder. 

    .
    ├── project                # Directory containing the source file for the project. This is the path provided
      

For each project a report is generated. They will be placed on the provided "path to report".

The last parameter is the report type, which can be either "XML" or "JSON". This is an optional parameter, with "JSON" being the default value. 

If no paramter is provided, the Attribute Sniffer will prompt the user to enter them.

Attribute Metrics
==================

The Attribute Sniffer was developed to validate Java annotations metrics on C# source code. It collects 5 attribute metrics. The original metrics were proposed and defined in the the paper
[A Metrics Suite for Code Annoation Assessment](https://www.sciencedirect.com/science/article/pii/S016412121730273X). This tool is part of an ongoing research regarding Java annotations and C# attributes.

### Collected metrics

* AC: Attributes in Class
* UAC: Unique Attributes in Class
* AED: Attributes in Element Declaration
* AA: Arguments in Attributes
* LOCAD: LOC in Attribute Declaration

