﻿global using AsmResolver.DotNet;
global using AsmResolver.DotNet.Cloning;
global using AsmResolver.DotNet.Code.Cil;
global using AsmResolver.DotNet.Signatures;
global using AsmResolver.PE.DotNet.Cil;
global using BitMono.Runtime;
global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Threading;
global using System.Threading.Tasks;
global using AsmResolver.DotNet.Code.Native;
global using AsmResolver.PE.File;
global using BitMono.API.Protections;
global using BitMono.Core;
global using BitMono.Core.Attributes;
global using BitMono.Core.Injection;
global using BitMono.Core.Renaming;
global using BitMono.Core.Services;
global using BitMono.Utilities.AsmResolver;
global using JetBrains.Annotations;
global using MethodAttributes = AsmResolver.PE.DotNet.Metadata.Tables.MethodAttributes;
global using MethodImplAttributes = AsmResolver.PE.DotNet.Metadata.Tables.MethodImplAttributes;
global using TypeAttributes = AsmResolver.PE.DotNet.Metadata.Tables.TypeAttributes;