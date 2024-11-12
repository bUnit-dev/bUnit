#/bin/bash

# Install docfx (should be aligned with docs-deploy.yml)
dotnet tool restore

# Trust dotnet developer certs
dotnet dev-certs https --check --trust
