---
uid: configure-3rd-party-libs
title: Configure 3rd Party Libraries for Testing
---

# Configure 3rd Party Libraries for Testing

Testing a component that is built using a third-party component library can require additional setup steps in each test to ensure that the third-party components work correctly in a test context.

For example, if a third-party component requires services to be registered in production code, it is likely that this is needed in a test context as well. See the <xref:inject-services> page for more info on this.

Similarly, if a root component is needed to provide cascading values to the third-party components, that is likely to be needed as well. For more on this, see the <xref:passing-parameters-to-components#cascading-parameters-and-cascading-values> page.

## bUnit Friendly Component Libraries

This section contains a table of third-party component libraries that are known for allowing their users to test their components with bUnit. If additional setup or configuration is needed, a link is provided to the component library's own documentation which shows how to test a component that uses components from the library.

| Component Library | bUnit compatibility notes | Test context setup guide |
| ----------------- | ------------------------- | -------------------------|
| Telerik UI for Blazor | [Feedback is appreciated](https://feedback.telerik.com/blazor) | [Telerik Grid Setup](https://www.telerik.com/blogs/unit-testing-blazor-components-bunit-justmock) |


> [!TIP]
> If you are a Blazor component vendor and have instructions on how to setup a bUnit test context for testing components that use your components, please add a link to your documentation and component library to the table above. 
> 
> Just submit a pull-request to this page with the relevant links added, share the relevant links in [bUnit's gitter chat](https://gitter.im/egil/bunit), or add an issue on [bUnit's github page](https://github.com/egil/razor-components-testing-library/issues) with the relevant links.
