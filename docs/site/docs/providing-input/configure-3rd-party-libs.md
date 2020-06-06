---
uid: configure-3rd-party-libs
title: Configure 3rd Party Libraries for Testing
---

# Configure 3rd Party Libraries for Testing

Testing a component that is built using a 3rd party component library can require additional setup steps in each test, to ensure the 3rd party components work correctly in a test context.

If for example a 3rd party component requires services to be registered in production code, it is likely that this is needed in a test context as well. See the <xref:inject-services> page for more info on this.

Similarly, if a root component is needed to provide cascading values to the 3rd party components, that is likely to be needed as well. See the <xref:passing-parameters-to-components#cascading-parameters-and-cascading-values> page for more on this.

> [!TIP]
> If you are a Blazor component vendor and have instructions on how to setup a bUnit test context for testing components that use your components, we would love to feature a link to your documentation and component library on this page. 
> 
> Just submit a pull-request to this page with the relevant links added, share the relevant links in [bUnit's gitter chat](https://gitter.im/egil/bunit), or add an issue on [bUnit's github page](https://github.com/egil/razor-components-testing-library/issues) with the relevant links.