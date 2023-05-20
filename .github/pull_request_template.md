## Pull request description
<!--- 
    Describe the changes and motivation behind them here, if it is not obvious
    from the related issues. Does it have new features, breaking changes, etc. 
-->

### PR meta checklist
- [ ] Pull request is targeted at `main` branch for code   
  or targeted at `stable` branch for documentation that is live on bunit.dev.
- [ ] Pull request is linked to all related issues, if any.
- [ ] I have read the _CONTRIBUTING.md_ document.

### Code PR specific checklist
- [ ] My code follows the code style of this project and AspNetCore coding guidelines.
- [ ] My change requires a change to the documentation.
  - [ ] I have updated the documentation accordingly.
- [ ] I have added new public API to the `PublicAPI.Unshipped.txt` and did not change the `PublicAPI.Shipped.txt` file.
- [ ] I have updated the appropriate sub section in the _CHANGELOG.md_.
- [ ] I have added, updated or removed tests to according to my changes.
  - [ ] All tests passed.
