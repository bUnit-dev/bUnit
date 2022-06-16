# Breaking changes

This document will list all notable breaking changes, making it easier to migrate.

## `v1` to `v2`
 - Majority of event overloads are removed. For example instead of `cut.Find("button").Click(detail: 3, ctrlKey: true)` use the event arguments directly provided by Microsoft: `cut.Find("button").Click(new MouseEventArgs { Detail = 3, CtrlKey = true });`