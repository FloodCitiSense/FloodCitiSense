import { FloodCitiSensePage } from './app.po';

describe('abp-zero-template App', function () {
    let page: FloodCitiSensePage;

    beforeEach(() => {
        page = new FloodCitiSensePage();
    });

    it('should display message saying app works', () => {
        page.navigateTo();
        page.getCopyright().then(value => {
            expect(value).toEqual(new Date().getFullYear() + ' © FloodCitiSense.');
        });
    });
});
