<template>
    <div class="col-md-4 mx-auto">
        <h1 class="text-center my-3">Create New Request</h1>
        <hr>
        <form method="post">
            <validation-error v-bind:errors="errors"></validation-error>
            <div class="form-group">
                <label class="h6">Request Type</label>
                <select v-model="requestTypeId" name="request-type" id="request-type" class="form-control" v-bind:class="{ invalid: $v.requestTypeId.$error }" v-on:blur="$v.requestTypeId.$touch()">
                     <option selected="selected">
                        -- Select Request Type --
                    </option>
                    <option v-for="requestType in requestTypes" v-bind:key="requestType.id" name="requestTypeId" v-bind:value="requestType.id">
                        {{ requestType.name }}
                    </option>
                </select>
                <p class="text-danger" v-if="$v.requestTypeId.$error && !$v.requestTypeId.required">The Request Type field is required.</p>
            </div>

            <div class="form-group" v-if="!isCreateRequest">
                <label for="accountIdentityNumber" class="h6">Account Identity Number</label>
                <select class="form-control" v-model="accountId">
                    <option v-for="account in accounts" v-bind:key="account.identityNumber" v-bind:value="account.id" name="accountId" v-bind:class="{ invalid: $v.accountId.$error }" v-on:blur="$v.accountId.$touch()">
                        {{ account.identityNumber }} ($ {{ account.balance | money }})
                    </option>
                </select>
                <p class="text-danger" v-if="$v.accountId.$error && !$v.accountId.required">The Account Identity Number field is required.</p>
            </div>

            <input type="submit" value="Submit" class="btn btn-success" v-on:click.prevent="makeRequest" />
        </form>
    </div>
</template>

<script>
import requestTypeService from '../../services/requestTypeService'
import exchangeAccountService from '../../services/exchangeAccountService'
import errorHandler from '../../helpers/error-handler'
import ValidationError from '../../components/shared/ValidationError'
import numeral from 'numeral'
import { validations } from '../../validations/requests/create'
import requestService from '../../services/requestService'

export default {
    components: {
        validationError: ValidationError
    },
    data: function () {
        return {
            requestTypeId: null,
            accountId: null,
            requestTypes: [],
            createRequestType: null,
            accounts: [],
            errors: []
        }
    },
    validations: validations,
    mounted: function () {
        var self = this;

        requestTypeService
            .getAll()
            .then(function ( response) {
                if (response && response.status === 200 && response.data) {
                    self.requestTypes = response.data.data;
                    self.createRequestType = response.data.data.filter(function (item) {
                        return item.name == 'Create Account';
                    })[0];
                }
            })
            .catch(function (error) {
                var errors = errorHandler.extractErrorsFromResponse(error.response);
                self.errors.push(errors);
            });

        exchangeAccountService
            .getActiveByClient()
            .then(function (response) {
                if (response && response.status === 200 && response.data) {
                    self.accounts = response.data;
                }
            })
            .catch(function (error) {
                var errors = errorHandler.extractErrorsFromResponse(error.response);
                self.errors.push(errors);
            });
    },
    computed: {
        isCreateRequest: function () {
            return !this.requestTypeId || (this.requestTypeId === this.createRequestType.id);
        }
    },
    methods: {
        makeRequest: function () {
            var self = this;

            this.$v.$touch()

            if (this.$v.$invalid) {
                return;
            }

            var data = { requestTypeId: this.requestTypeId };

            if (!this.isCreateRequest) {
                data.accountId = this.accountId;
            }

            requestService.create(data)
            .then(function () {
                self.$router.push("/");
            })
            .catch(function (error) {
                self.errors = errorHandler.extractErrorsFromResponse(error.response);
            });
        }
    },
    filters: {
        money: function (value) {
            if (!value) { return ''; }

            return numeral(value).format('0,0');
        }
    }
}
</script>