import { required, requiredIf } from 'vuelidate/lib/validators'

export const validations = {
    requestTypeId: {
        required: required
    },
    accountId: {
        required: requiredIf(function () {
            return !this.isCreateRequest;
        })
    },
    accountTypeId: {
        requried: requiredIf(function () {
            return this.isCreateRequest;
        })
    }
}