import { required } from 'vuelidate/lib/validators'

export const validations = {
    senderAccount: {
        required: required
    },
    receiverAccount: {
        required: required
    },
    amount: {
        required: required
    },
    description: {
        required: required
    }
}