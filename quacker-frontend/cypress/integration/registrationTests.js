import faker from 'faker';

var baseURL = 'https://staging.quacker.nl/';

describe('Register account', () => {
  it('Should throw error if password has no number', () => {
    cy.visit(baseURL + 'login');

    cy.get('a[id=signup]').click();

    cy.get('input[id=registration-username-input]').type(
      faker.internet.userName()
    );
    cy.get('input[id=registration-email-input]').type(faker.internet.email());
    cy.get('input[id=registration-password-input]').type('test');
    cy.contains('The password field must have numbers');
  });
});
