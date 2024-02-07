import faker from 'faker';

var baseURL = 'https://staging.quacker.nl/';

var admin = {
  username: 'admin',
  password: 'Admin123',
};

describe('Posting quacks', () => {
  it('Post quack should show quack in timeline', () => {
    cy.login(admin.username, admin.password);

    var message = faker.random.words();

    cy.wait(2000);

    cy.get('textarea').type(message);
    cy.get('button[id=quack-button]').click({ force: true });

    cy.wait(2000);

    cy.contains(message);
  });
});
