import { required } from 'vuelidate/lib/validators'

export const validations = {
    username: {
        required: required
    },
    password: {
        required: required
    }
}