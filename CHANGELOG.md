# Changelog

All notable changes to this project are documented here.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

### Added
- Rewritten README with architecture diagram, quick start and route tables.
- `.editorconfig`, `Directory.Build.props`, `.env.example`.
- Pull request and bug report templates, `CODEOWNERS`, Dependabot configuration.
- Solution-wide CI workflow (build, format check, tests).

### Changed
- Docker Compose credentials moved to a git-ignored `.env` file.
- Helper database scripts read credentials from `.env`.
- Renamed `Customer.Application/Contracts/Persistance` to `Persistence`.
- Normalised code formatting and removed a personal email from Swagger metadata.

## [ApiVersioningLib 1.1.3]

- See [ApiVersioningLib README](src/NugetPackage/ApiVersioningLib/README.md).
