# Contributing

Thanks for your interest in improving this project. By participating you agree to follow the
[Code of Conduct](CODE_OF_CONDUCT.md).

## Reporting issues

Open an issue using the bug report or feature request template and include:
- a clear description of the problem or idea,
- steps to reproduce (for bugs) and expected vs. actual behaviour,
- logs, screenshots or error messages where relevant.

Security vulnerabilities must be reported privately, see [SECURITY.md](SECURITY.md).

## Development setup

```bash
cd src
cp .env.example .env
docker compose up -d --build      # full system
dotnet build MicroservicesArchitecture.sln
dotnet test Services/Account/Account.Test/Account.Test.csproj
```

Formatting is defined in [`.editorconfig`](.editorconfig). Before pushing:

```bash
cd src
dotnet format whitespace MicroservicesArchitecture.sln --exclude '**/Migrations/**'
```

## Branch naming

Branch from `develop` using `<type>/<short-kebab-description>`:

| Type | Use for | Example |
|---|---|---|
| `feat/` | New functionality | `feat/account-transfer` |
| `fix/` | Bug fixes | `fix/withdraw-negative-balance` |
| `docs/` | Documentation only | `docs/readme-overhaul` |
| `refactor/` | Code changes without behaviour change | `refactor/transaction-repository` |
| `test/` | Adding or fixing tests | `test/customer-handlers` |
| `chore/` | Tooling, dependencies, CI | `chore/bump-masstransit` |

## Commit messages

This repository uses [Conventional Commits](https://www.conventionalcommits.org/):

```text
<type>(<optional scope>): <imperative summary>
```

Examples: `feat(account): add transfer between accounts`, `fix(transaction): handle empty filter`,
`chore: bump MassTransit to 8.2.6`.

## Pull requests

1. Open the PR against `develop`.
2. Keep it focused; fill in the PR template.
3. Make sure CI is green (build, format check, tests).
4. Update `CHANGELOG.md` under *Unreleased* for user-visible changes.
5. If you change a controller route, update the Ocelot configuration too.
