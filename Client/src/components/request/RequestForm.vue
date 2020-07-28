<template>
    <div class="col-sm-6 col-md-5 col-xl-3 mx-auto">
        <h1 class="text-center my-3">Create New Request</h1>
        <hr>
        <form method="post">
            <validation-error v-bind:errors="errors"></validation-error>
            <div class="form-group">
                <label class="h6">Request Type</label>
                <select v-model="requestTypeId" v-on:change="loadAccountTypes" name="request-type" id="request-type" class="form-control" v-bind:class="{ invalid: $v.requestTypeId.$error }" v-on:blur="$v.requestTypeId.$touch()">
                     <option selected="selected" name="requestTypeId" v-bind:value="null">
                        -- Select Request Type --
                    </option>
                    <option v-for="requestType in requestTypes" v-bind:key="requestType.id" name="requestTypeId" v-bind:value="requestType.id">
                        {{ requestType.name }}
                    </option>
                </select>
                <p class="text-danger" v-if="$v.requestTypeId.$error && !$v.requestTypeId.required">The Request Type field is required.</p>
            </div>

            <div class="form-group" v-show="isCreateRequest">
                <label for="accountType" class="h6">Account Type</label>
                <select class="form-control" v-model="accountTypeId">
                    <option v-for="(accountType, index) in accountTypes" v-bind:key="index" v-bind:value="accountType.id" name="accountTypeId" v-bind:class="{ invalid: $v.accountTypeId.$error }" v-on:blur="$v.accountTypeId.$touch()">
                        {{ accountType.name }}
                    </option>
                </select>
                <p class="text-danger" v-if="$v.accountTypeId.$error && !$v.accountTypeId.required">The Account Type field is required.</p>
            </div>

            <div class="form-group" v-show="requestTypeId && !isCreateRequest">
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
import requestService from '../../services/requestService'
import exchangeAccountTypeService from '../../services/exchangeAccountTypeService'

import errorHandler from '../../helpers/error-handler'
import ValidationError from '../../components/shared/ValidationError'
import money from '../../filters/money'

import { validations } from '../../validations/requests/create'

export default {
    components: {
        validationError: ValidationError
    },
    data: function () {
        return {
            requestTypeId: null,
            accountId: null,
            accountTypeId: null,
            requestTypes: [],
            accountTypes: [],
            createRequestType: null,
            accounts: [],
            errors: []
        }
    },
    validations: validations,
    filters: {
        money
    },
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
            return this.requestTypeId && (this.requestTypeId === this.createRequestType.id);
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
            } else {
                data.accountTypeId = this.accountTypeId;
            }

            requestService.create(data)
            .then(function () {
                self.$router.push("/");
            })
            .catch(function (error) {
                self.errors = errorHandler.extractErrorsFromResponse(error.response);
            });
        },
        loadAccountTypes: function () {
            var self = this;

            if (this.isCreateRequest) {
                exchangeAccountTypeService
                    .getAll()
                    .then(function (response) {
                        if (response && response.status == 200) {
                            self.accountTypes = response.data;
                        }
                    })
                    .catch(function (error) {
                        self.errors = errorHandler.extractErrorsFromResponse(error.response);
                    });
            } else {
                self.accountTypeId = null;
            }
        }
    }
}
</script>