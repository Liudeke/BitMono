﻿namespace BitMono.CLI.Modules;

internal class ReadlineObfuscationNeedsFactory
{
    private readonly string[] _args;
    private readonly ObfuscationSettings _obfuscationSettings;
    private readonly ILogger _logger;

    public ReadlineObfuscationNeedsFactory(string[] args, ObfuscationSettings obfuscationSettings, ILogger logger)
    {
        _args = args;
        _obfuscationSettings = obfuscationSettings;
        _logger = logger.ForContext<ReadlineObfuscationNeedsFactory>();
    }

    public ObfuscationNeeds Create(CancellationToken cancellationToken)
    {
        var fileName = GetFileName(_args);
        while (true)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger.Information("Please, specify file or drag-and-drop in BitMono CLI");

                fileName = PathFormatterUtility.Format(Console.ReadLine());
                cancellationToken.ThrowIfCancellationRequested();
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    if (File.Exists(fileName))
                    {
                        _logger.Information("File successfully specified: {0}", fileName);
                        break;
                    }

                    _logger.Warning("File cannot be found, please, try again!");
                }
                else
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    _logger.Warning("Unable to specify empty null or whitespace file, please, try again!");
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Something went wrong while specifying the file");
            }
        }

        string dependenciesDirectoryName;
        string outputDirectoryName;
        var fileBaseDirectory = Path.GetDirectoryName(fileName);
        if (_obfuscationSettings.ForceObfuscation)
        {
            dependenciesDirectoryName = fileBaseDirectory;
            outputDirectoryName = fileBaseDirectory;
        }
        else
        {
            outputDirectoryName = Path.Combine(fileBaseDirectory, _obfuscationSettings.OutputDirectoryName);
            dependenciesDirectoryName = Path.Combine(fileBaseDirectory, _obfuscationSettings.ReferencesDirectoryName);
            if (!Directory.Exists(dependenciesDirectoryName))
            {
                while (true)
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (Directory.Exists(dependenciesDirectoryName))
                        {
                            _logger.Information("Dependencies (libs) successfully found automatically: {0}!",
                                dependenciesDirectoryName);
                            break;
                        }

                        _logger.Information("Please, specify dependencies (libs) path: ");
                        var newDependenciesDirectoryName = PathFormatterUtility.Format(Console.ReadLine());
                        if (!string.IsNullOrWhiteSpace(newDependenciesDirectoryName))
                        {
                            if (Directory.Exists(newDependenciesDirectoryName))
                            {
                                dependenciesDirectoryName = newDependenciesDirectoryName;
                                _logger.Information("Dependencies (libs) successfully specified: {0}!",
                                    newDependenciesDirectoryName);
                                break;
                            }
                            else
                            {
                                _logger.Information("Libs directory doesn't exist, please, try again!");
                            }
                        }
                        else
                        {
                            _logger.Information("Unable to specify empty (libs), please, try again!");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Something went wrong while specifying the dependencies (libs) path");
                    }
                }
            }
            else
            {
                _logger.Information("Dependencies (libs) directory was automatically found in: {0}!",
                    dependenciesDirectoryName);
            }
        }

        Directory.CreateDirectory(outputDirectoryName);
        Directory.CreateDirectory(dependenciesDirectoryName);
        return new ObfuscationNeeds
        {
            FileName = fileName,
            FileBaseDirectory = fileBaseDirectory,
            ReferencesDirectoryName = dependenciesDirectoryName,
            OutputPath = outputDirectoryName,
            Way = ObfuscationNeedsWay.Readline
        };
    }

    private string GetFileName(string[] args)
    {
        string? file = null;
        if (!args.IsEmpty())
        {
            file = PathFormatterUtility.Format(args[0]);
        }
        return file;
    }
}