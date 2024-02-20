# Optimizely Graph Client SDKs (Internal)

This README contains internal guidelines for managing the Optimizely Graph Client SDK repository. For general information about the project, building, testing, and public contribution guidelines, refer to the [Public README.md](https://github.com/episerver/graph-net-sdk).

## Handling External Pull Requests

- **Review Process**: All external pull requests must be reviewed for quality, security, and compliance with our project standards.
- **Testing**: As the community will primarily own testing, ensure that all external contributions come with full test coverage and have been tested locally by the contributor.

## Decision Making for Builds and Releases

- **Building a .NET Package**: Decisions on when to build a new package will depend on significant updates or fixes that impact the functionality or performance of the SDK.
- **Releases and NuGet Publishing**:
  - **GitHub Release**: Run the GitHub action for creating a release when a stable version has been tested and is ready for production use.
  - **NuGet Publishing**: The action to publish to NuGet should be run post-release, ensuring the package is available for wider consumption.

## Security Measures for GitHub Actions

- **Restricted Actions**: Ensure that only internal team members can trigger actions for building, releasing, publishing packages and running tests. This can be managed through GitHub action permissions and repository settings.

## QA and External Contributions

- **Quality Assurance**: As external contributions will not go through an internal QA process, it is crucial to ensure contributions meet our testing and quality standards before merging.

For any questions or further clarifications, please contact the repository administrators.

## Releases
### Create a pre-release

 1. Create a new branch with name `release/$version_release`
 2. Update version for package in `msbuild/version.props` file under `PropertyGroup` tag
 3. Commit, push, and create PR to `release/$version_release` branch
 4. Goto GitHub Actions [Build & Release](https://github.com/episerver/graph-net-sdk/actions/workflows/createRelease.yml)
 5. Choose `Build` under `Build Options` and `PRE-RELEASE(release/**)` option under `Release Options`
 6. Hit `Run workflow` to build and create a pre-release package
### Create a release

 1. Update version for package in `msbuild/version.props` file under `PropertyGroup` tag
 2. Commit, push, and create PR to `master` branch
 3. Goto GitHub Actions [Build & Release](https://github.com/episerver/graph-net-sdk/actions/workflows/createRelease.yml)
 4. Choose `Build + Upload T3` under `Build Options` and `RELEASE(master branch)` option under `Release Options`
 5. Hit `Run workflow` to build and create a release package. This package then will be uploaded to `Release this week` on T3
