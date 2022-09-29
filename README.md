# Amusoft.Toolkit.Http

## Project state
[![.GitHub](https://github.com/taori/Amusoft.Toolkit.Http/actions/workflows/dotnet.yml/badge.svg)](https://github.com/taori/Amusoft.Toolkit.Http/actions/workflows/dotnet.yml)
[![GitHub issues](https://img.shields.io/github/issues/taori/Amusoft.Toolkit.Http)](https://github.com/taori/Amusoft.Toolkit.Http/issues)
[![NuGet version (Amusoft.Templates)](https://img.shields.io/nuget/v/Amusoft.Toolkit.Http.svg)](https://www.nuget.org/packages/Amusoft.Toolkit.Http/)


## Description

This project contains a toolkit for the usage area of HTTP functionality

# Features

- Downloading files with progress callbacks
    - `DownloadWithProgress.ExecuteAsync(HttpClient, downloadUrl, destinationPath, progress, requestMessageBuilder, cancellationToken)`
    - `HttpClient.DownloadWithProgressAsync(downloadUrl, destinationPath, progress, requestMessageBuilder, cancellationToken)` 