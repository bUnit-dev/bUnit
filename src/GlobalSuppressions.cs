// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
using System.Diagnostics.CodeAnalysis;
[assembly: SuppressMessage("Usage", "BL0006:Do not use RenderTree types", 
    Justification = "<Pending>", 
    Scope = "namespaceanddescendants", 
    Target = "Egil.RazorComponents.Testing")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", 
    Justification = "No need to translate at this point", Scope = "namespaceanddescendants", Target = "Egil.RazorComponents.Testing")]
[assembly: SuppressMessage("Design", "CA1032:Implement standard exception constructors", Scope = "namespaceanddescendants", Target = "Egil.RazorComponents.Testing")]
