import { required, between } from 'vuelidate/lib/validators'

export const validations = {
    cardNumber: {
        required,
        length: function (val) {
            if (!val) {
                return false;
            }

            return val.length == 12;
        }
    },
    amount: {
        required,
        between: between(1, 10000)
    },
    accountId: {
        required
    }
};