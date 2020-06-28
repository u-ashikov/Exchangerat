import { required, between, maxLength } from 'vuelidate/lib/validators'

export const validations = {
    senderAccount: {
        required: required
    },
    receiverAccount: {
        required: required
    },
    amount: {
        required: required,
        between: between(1, 10000)
    },
    description: {
        required: required,
        maxLength: maxLength(300)
    }
}