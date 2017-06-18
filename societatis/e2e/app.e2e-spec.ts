import { SocietatisPage } from './app.po';

describe('societatis App', () => {
  let page: SocietatisPage;

  beforeEach(() => {
    page = new SocietatisPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
