<template>
     <div>
        <h1 class="text-center dispay-4 mt-3 mb-5">Add Funds</h1>
        <form method="post" class="col-sm-6 col-md-5 col-xl-3 mx-auto" v-on:submit.prevent="sendFunds">
            <validation-error v-bind:errors="errors"></validation-error>
            <div class="form-group">
                <label class="h6" for="card-number">Card Number</label>
                <input type="text" name="card-number" id="card-number" class="form-control" v-model.lazy="cardNumber" v-bind:class="{ invalid: $v.cardNumber.$error }" v-on:blur="$v.cardNumber.$touch()" />
                <p class="text-danger" v-if="$v.cardNumber.$error && !$v.cardNumber.required">The Card Number field is required.</p>
                <p class="text-danger" v-if="$v.cardNumber.$error && !$v.cardNumber.length">The Card Number field must be exactly 12 symbols length.</p>
            </div>
            <div class="form-group">
                <label class="h6" for="account">Account</label>
                <select name="account" id="account" v-model="accountId" class="form-control" v-bind:class="{ invalid: $v.accountId.$error }" v-on:blur="$v.accountId.$touch()">
                    <option v-for="account in accounts" v-bind:key="account.id" name="accountId" v-bind:value="account.id">{{ account.identityNumber}} ($ {{ account.balance | money }})</option>
                </select>
                <p class="text-danger" v-if="$v.accountId.$error && !$v.accountId.required">The Account field is required.</p>
            </div>

            <div class="form-group">
                <label class="h6" for="amount">Amount</label>
                <input type="number" name="amount" id="amount" min="0" step="0.01" class="form-control" v-model.lazy="amount" v-bind:class="{ invalid: $v.amount.$error }" v-on:blur="$v.amount.$touch()" />
                <p class="text-danger" v-if="$v.amount.$error && !$v.amount.required">The Amount field is required.</p>
                <p class="text-danger" v-if="$v.amount.$error && !$v.amount.between">The Amount must be between {{ $v.amount.$params.between.min }} and {{ $v.amount.$params.between.max }} inclusive.</p>
            </div>

            <input type="submit" value="Send" class="btn btn-success">
        </form>
    </div>
</template>

<script>
import fundService from '../../services/fundService'
import exchangeAccountService from '../../services/exchangeAccountService'
import numeral from 'numeral'

import ValidationError from "../shared/ValidationError"
import { validations } from '../../validations/funds/add'

import errorHandler from '../../helpers/error-handler'

export default {
    components: {
        validationError: ValidationError
    },
    data: function () {
        return {
            cardNumber:'',
            amount: 0,
            accountId: null,
            accounts: [],
            errors: []
        }
    },
    validations: validations,
    mounted: function () {
        var self = this;

        self.errors = [];

        exchangeAccountService.getActiveByClient()
        .then(function (response) {
            if (response && response.status == 200) {
                self.accounts = response.data;

                self.errors = [];
            }
        })
        .catch(function (error) {
            self.errors = errorHandler.extractErrorsFromResponse(error.response);
        });
    },
    methods: {
        sendFunds: function () {
            var self = this;

            this.$v.$touch();

            if (this.$v.$invalid) {
              return;
            }

            fundService.add({ accountId: this.accountId, amount: parseFloat(this.amount), cardIdentityNumber: this.cardNumber })
            .then(function (response) {
                if (response && response.status == 200) {
                    self.errors = [];

                    self.$router.push('/');
                }
            })
            .catch(function (error) {
                self.errors = errorHandler.extractErrorsFromResponse(error.response);
            })
        }
    },
    filters: {
       money: function (value) {
            if (!value) {
                return 0;
            }

            return numeral(value).format('0,0');
        },
    }
}
</script>