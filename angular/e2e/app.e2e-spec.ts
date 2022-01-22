import { VendingMachineSystemTemplatePage } from './app.po';

describe('VendingMachineSystem App', function() {
  let page: VendingMachineSystemTemplatePage;

  beforeEach(() => {
    page = new VendingMachineSystemTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
