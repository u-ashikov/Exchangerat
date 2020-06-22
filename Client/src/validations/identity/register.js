import { required, minLength, maxLength, email, sameAs } from 'vuelidate/lib/validators'

export const validations = {
    username: {
        required,
        minLength: minLength(4),
        maxLength: maxLength(20)
    },
    email: {
        required: required,
        minLength: minLength(5),
        maxLength: maxLength(80),
        email
    },
    password: {
        required,
        minLength: minLength(6),
        maxLength: maxLength(10)
    },
    confirmPassword: {
        required,
        sameAs: sameAs('password')
    },
    firstName: {
        required,
        minLength: minLength(1),
        maxLength: maxLength(20)
    },
    lastName: {
        required,
        minLength: minLength(1),
        maxLength: maxLength(20)
    },
    address: {
        required,
        minLength: minLength(4),
        maxLength: maxLength(100)
    }
}