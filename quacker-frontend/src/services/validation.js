import { extend } from 'vee-validate';
import { required, email, image, ext, size } from 'vee-validate/dist/rules';
import { configure } from 'vee-validate';

configure({
  classes: {
    invalid: 'is-invalid',
    dirty: ['is-dirty', 'is-dirty'],
  },
});

extend('confirm', {
  params: ['target'],
  validate(value, { target }) {
    return value === target;
  },
  message: 'Password confirmation does not match',
});

extend('email', {
  ...email,
  message: 'This is not a valid email',
});

extend('required', {
  ...required,
  message: 'The {_field_} field is required',
});

extend('ext', {
  ...ext,
  message: 'This file type is not supported',
});

extend('image', {
  ...image,
  message: 'This file type is not supported',
});

extend('size', {
  ...size,
  message: 'File must be less than 40MB',
});

extend('min', {
  validate(value, args) {
    return value.length >= args.length;
  },
  params: ['length'],
  message: 'The {_field_} field must have at least {length} characters',
});

extend('containLetters', {
  validate(value) {
    return /[a-zA-Z]/g.test(value);
  },
  message: 'The {_field_} field must have letters',
});

extend('containNumbers', {
  validate(value) {
    return /\d/.test(value);
  },
  message: 'The {_field_} field must have numbers',
});

extend('uppercase', {
  validate(value) {
    return /[A-Z]/.test(value);
  },
  message: 'The {_field_} field must have an uppercase character',
});

extend('lowercase', {
  validate(value) {
    return /[a-z]/.test(value);
  },
  message: 'The {_field_} field must have a lowercase character',
});

extend('decimal', {
  validate: (value, { decimals = '*', separator = '.' } = {}) => {
    if (value === null || value === undefined || value === '') {
      return {
        valid: false,
      };
    }
    if (Number(decimals) === 0) {
      return {
        valid: /^-?\d*$/.test(value),
      };
    }
    const regexPart = decimals === '*' ? '+' : `{1,${decimals}}`;
    const regex = new RegExp(
      `^[-+]?\\d*(\\${separator}\\d${regexPart})?([eE]{1}[-]?\\d+)?$`
    );

    return {
      valid: regex.test(value),
    };
  },
  message: 'The {_field_} field must contain only decimal values',
});
