<template>
    <div>
        <h1 class="text-center dispay-4 mt-3 mb-5">Create Transaction</h1>
        <form method="post" class="col-4 mx-auto" v-on:submit.prevent="sendTransaction">
            <validation-error v-bind:errors="errors"></validation-error>
            <div class="form-group">
                <label class="h6" for="sender-account">Sender Account</label>
                <select id="sender-account" v-model="senderAccount" class="form-control">
                    <option v-for="account in userActiveAccounts" v-bind:key="account.id" v-bind:value="account.identityNumber">{{ account.identityNumber }} ($ {{ account.balance | money }})</option>
                </select>
                <p class="text-danger" v-if="$v.senderAccount.$error && !$v.senderAccount.required">The Sender Account field is required.</p>
            </div>
            <div class="form-group">
                <label class="h6" for="receiver-account">Receiver Account</label>
                <input type="text" name="receiver-account" id="receiver-account" class="form-control" v-model.lazy="receiverAccount" v-bind:class="{ invalid: $v.receiverAccount.$error }" v-on:blur="$v.receiverAccount.$touch()" />
                <p class="text-danger" v-if="$v.receiverAccount.$error && !$v.receiverAccount.required">The Receiver Account field is required.</p>
            </div>

            <div class="form-group">
                <label class="h6" for="amount">Amount</label>
                <input type="number" name="amount" id="amount" min="0" step="0.01" class="form-control" v-model.lazy="amount" v-bind:class="{ invalid: $v.amount.$error }" v-on:blur="$v.amount.$touch()" />
                <p class="text-danger" v-if="$v.amount.$error && !$v.amount.required">The Amount field is required.</p>
                <p class="text-danger" v-if="$v.amount.$error && !$v.amount.between">The Amount must be between {{ $v.amount.$params.between.min }} and {{ $v.amount.$params.between.max }} inclusive.</p>
            </div>

            <div class="form-group">
                <label class="h6" for="description">Description</label>
                <textarea name="description" id="description" cols="10" rows="4" class="form-control" v-model.lazy="description" v-bind:class="{ invalid: $v.description.$error }" v-on:blur="$v.description.$touch()"></textarea>
                <p class="text-danger" v-if="$v.description.$error && !$v.description.required">The Description field is required.</p>
                <p class="text-danger" v-if="$v.description.$error && !$v.description.maxLength">The Description can be at most {{ $v.description.$params.maxLength }} symbols length.</p>
            </div>

            <input type="submit" value="Send" class="btn btn-success">
        </form>
    </div>
</template>

<script>
import exchangeAccounts from '../../queries/exchangeAccounts.js'
import transactions from '../../queries/transactions.js'
import { validations } from '../../validations/transactions/create'
import errorHandler from '../../helpers/error-handler'
import numeral from 'numeral'
import ValidationError from '../../components/shared/ValidationError'

export default {
    components: {
        validationError: ValidationError
    },
    data: function () {
        return {
            senderAccount: '',
            receiverAccount: '',
            amount: 0,
            description: '',
            userActiveAccounts: [],
            errors: []
        }
    },
    validations: validations,
    methods: {
        sendTransaction: function () {
            var self = this;
            this.errors = [];

            this.$v.$touch();
        
            if (this.$v.$invalid) {
              return;
            }

            transactions.create({ senderAccount: this.senderAccount, receiverAccount: this.receiverAccount, amount: parseFloat(this.amount), description: this.description })
                .then(function (response) {
                    if (response && response.status === 200) {
                        self.errors = [];
                        self.$router.push("/");
                    }
                })
                .catch(function (error) {
                    self.errors = errorHandler.extractErrorsFromResponse(error.response);
                });
        }
    },
    filters: {
        money: function (value) {
            if (!value) { return '' }

            return numeral(value).format('0,0');
        }
    },
    mounted: function () {
        var self = this;

        exchangeAccounts.getActiveByClientForTransaction()
            .then(function (response) {
                if (response && response.data) {
                    self.userActiveAccounts = response.data;
                }
            })
            .catch(function (error) {
                self.errors = errorHandler.extractErrorsFromResponse(error.response);
            });
    }
}
</script>