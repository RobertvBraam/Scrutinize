# Scrutinize

## Introduction

Welcome to Scrutinize - Your Open Source Software (OSS) companion for comprehensive dependency analysis. Scrutinize is a powerful and extensible tool designed to scan and analyze software projects utilizing npm and NuGet for their dependencies. This documentation will guide you through the setup, features, and best practices to make the most out of Scrutinize in ensuring the security and health of your software projects.

## Table of Contents:
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Configuration](#configuration) (TODO)
- [Features](#features)
  - [Vulnerability Assessment](#vulnerability-assessment)
  - [License Compliance](#license-compliance)
- [Usage](#usage) (TODO)
  - [Command Line Interface](#command-line-interface) (TODO)
  - [Integration with CI/CD](#integration-with-cicd) (TODO)
- [Extensibility](#extensibility) (TODO)
  - [Plugins](#plugins) (TODO)
  - [Custom Rules](#custom-rules) (TODO)
- [Best Practices](#best-practices) (TODO)
  - [Regular Scans](#regular-scans) (TODO)
  - [Automated Reporting](#automated-reporting) (TODO)
  - [Collaborative Analysis](#collaborative-analysis) (TODO)
- [Contributing](#contributing) (TODO)
  - [Reporting Issues](#reporting-issues) (TODO)
  - [Feature Requests](#feature-requests) (TODO)
  - [Code Contributions](#code-contributions) (TODO)
- [License](#license) (TODO)

## Getting Started

### Installation

To install Scrutinize, you only need to install npm (v9.4+) and nuget(v7.0+) and the following packages/tools: [license-checker](https://www.npmjs.com/package/license-checker) and [dotnet-project-licenses](https://www.nuget.org/packages/dotnet-project-licenses/2.7.1).

Commands to install the tools (execute in the `./src` directory):
- `npm install license-checker`
- `dotnet new tool-manifest --force && dotnet tool install dotnet-project-licenses`

### Configuration

Configure Scrutinize to meet the specific needs of your project. Learn how to set up and customize Scrutinize by referring to the Configuration Guide.

## Features

### Vulnerability Assessment

The vulnerability assessment is done by scanning the dependencies of specified project with the tooling natively integrated by npm (e.g. `npm audit`) and Nuget (e.g. `dotnet list package --vulnerable`). Both package managers are based on the Github Advisory Database ([npm](https://github.blog/2021-10-07-github-advisory-database-now-powers-npm-audit/) and [Nuget](https://devblogs.microsoft.com/nuget/how-to-scan-nuget-packages-for-security-vulnerabilities/)) to determine what vulnerabilities are present in the packages. The assessment will output a list of vulnerabilities contianing the package name, version, severity and the vulnerability reference url.

### License Compliance

Licenses are being scanned using open source tooling to analyse if dependencies are compliant to the licenses you allow. Tooling that are used: for npm [license-checker](https://www.npmjs.com/package/license-checker) and for Nuget [dotnet-project-licenses](https://www.nuget.org/packages/dotnet-project-licenses/2.7.1). The output of a License scan will be a list of the package name, version and it's license type.

## Usage

### Command Line Interface (CLI)

Learn how to use Scrutinize's command-line interface for manual scans and integration into your development workflow. Find detailed instructions in the CLI Guide by calling `scrutinize -h`.

### Integration with CI/CD

Integrate Scrutinize into your Continuous Integration/Continuous Deployment (CI/CD) pipeline to automate dependency analysis. Follow the steps outlined in the CI/CD Integration Guide.

Azure DevOps example to run Scrutinize in your pipeline (untested):
```yaml
- task: Npm@1
  displayName: 'Install npm dependencies'
  inputs:
    command: 'install license-checker'
    verbose: false
- task: dotnet@2
  displayName: 'Install local manifest'
  inputs:
      command: 'custom'
      custom: 'new'
      arguments: 'tool-manifest --force'
- task: dotnet@2
  displayName: 'Install dotnet-project-licenses tool'
  inputs:
    command: 'custom'
    custom: 'tool'
    arguments: 'install dotnet-project-licenses'
- task: Nuget@1
  displayName: 'Install Scrutinize tool'
  inputs:
    command: 'custom'
    arguments: 'tool install Scrutinize'
```

## Best Practices

### Regular Scans

Perform regular scans to stay up-to-date with the latest dependencies and potential security vulnerabilities.

### Automated Reporting

Automate the reporting process to receive timely updates on the health and security of your software projects.

### Collaborative Analysis

Encourage collaboration by sharing reports and findings with your development team and stakeholders.

## Contributing

### Reporting Issues

Help us improve Scrutinize by reporting any issues you encounter. Follow the guidelines in the Issue Reporting Guide.

### Feature Requests

Suggest new features or improvements by following the instructions in the Feature Request Guide.

### Code Contributions

Contribute to the development of Scrutinize by following the guidelines in the Contribution Guide.

## License

Scrutinize is released under the GPL-3.0 license. Please review the license file for more details on how you can use and contribute to this open-source project.