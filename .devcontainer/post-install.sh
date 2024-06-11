#/bin/bash

# Install docfx (should be aligned with docs-deploy.yml)
dotnet tool install --global docfx --version 2.74.1

# Trust dotnet developer certs
dotnet dev-certs https --check --trust
