# Optimizely Graph Client SDKs

This repository contains the source code for the Optimizely Graph Client. This project is tent to build GraphQL queries by several lines of lambda expression in C#.
The main purpose of this project is support Optimizely's customers to build the search query to [Optimizely Graph](https://docs.developers.optimizely.com/platform-optimizely/v1.4.0-optimizely-graph/docs/project-graphql) that similar to [Search & Navigation](https://docs.developers.optimizely.com/digital-experience-platform/v1.1.0-search-and-navigation/docs/net-client-api).

## Optimizely Graph

* [Documentation](https://docs.developers.optimizely.com/platform-optimizely/v1.4.0-optimizely-graph/docs/project-graphql)

## Optimizely Graph Client

* [Documentation](https://docs.developers.optimizely.com/platform-optimizely/v1.4.0-optimizely-graph/docs/introduction)

## Prerequisites for building and testing the SDKs, sample site

This project uses:
* .NET SDK 6+
* SQL Server 2016 Express LocalDB ([download here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads))

## Create a pre-release

 1. Create a new branch with name `release/$version_release`.
 2. Update version for package in `msbuild/version.props` file under `PropertyGroup` tag.
 3. Commit, push, and create PR to `release/$version_release` branch.
 4. Goto GitHub Actions then click `Build & Release` flow under `Use workflow from`.
 5. Choose `Build` under `Build Options` and `PRE-RELEASE(release/**)` option under `Release Options`.
 6. Hit `Run workflow` to build and create a pre-release package.

## Create a release

 1. Update version for package in `msbuild/version.props` file under `PropertyGroup` tag.
 2. Commit, push, and create PR to `master` branch.
 3. Goto GitHub Actions then click `Build & Release` flow under `Use workflow from`.
 4. Choose `Build + Upload T3` under `Build Options` and `RELEASE(master branch)` option under `Release Options`.
 5. Hit `Run workflow` to build and create a release package. This package then will be uploaded to `Release this week` on T3.

## Contributing

The easiest way to contribute is to join in with the discussions on Github issues or create new issues with questions, suggestions or any other feedback. If you want to contribute code or documentation, you are more than welcome to create pull-requests, but make sure that you read the contribution page first.