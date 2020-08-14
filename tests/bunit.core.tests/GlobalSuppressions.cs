// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "BL0006:Do not use RenderTree types")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "not in tests")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>")]
[assembly: SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "<Pending>")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "In tests its ok to catch the general exception type")]
[assembly: SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings", Justification = "<Pending>")]
[assembly: SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
[assembly: SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes")]
