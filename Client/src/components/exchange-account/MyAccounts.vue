<template>
<div class="container">
    <h1 class="text-center display-4 mt-3 mb-5">My Accounts</h1>
    <validation-error v-bind:errors="errors" v-cloak></validation-error>
    <div class="row" v-show="accounts && accounts.length > 0" v-cloak>
        <div v-for="account in accounts" v-bind:key="account.accountNumber" class="col-sm-5 mb-3 mx-auto">
            <div class="card">
              <div class="card-body">
                <h5 class="card-title">{{ account.accountNumber }}</h5>
                <p class="card-text">
                    <i class="fas fa-money-check-alt"></i> {{ account.type }} 
                    <span class="d-block"><i class="fas fa-dollar-sign"></i> {{ account.balance | money }}</span>
                    <span class="d-block"><i class="fas fa-calendar-week"></i> {{ account.createdAt }}</span>
                    <span v-if="account.isActive" class="d-block"><i class="fas fa-check text-success"></i> Active</span>
                    <span v-else class="d-block"><i class="fas fa-times text-danger"></i> Inactive</span>
                </p>
                <router-link class="btn btn-primary" tag="a" :to="{ name: 'accountTransactions', params: { accountId:  account.id } }">Transactions</router-link>
              </div>
            </div>
        </div>
    </div>
    <div v-show="accounts && accounts.length === 0 && errors.length === 0" class="container w-50 alert alert-success mx-auto" role="alert" v-cloak>
        <h4 class="alert-heading">Ooooops!</h4>
        <p>Sorry, you don't have any accounts yet. If you want you can request one by using the link bellow.</p>
        <hr>
        <router-link tag="a" class="btn btn-link" :to="{ name: 'requestAccount' }">Request Account</router-link>
    </div>
    </div>
</template>

<script>
import ValidationError from '../../components/shared/ValidationError';

import exchangeAccounts from '../../queries/exchangeAccounts.js';

import { authHeader } from '../../helpers/auth-header.js';
import { errorHandler } from '../../helpers/error-handler';

import numeral from 'numeral';

export default {
    components: {
        validationError: ValidationError
    },
    data: function () {
        return {
            accounts: null,
            errors: []
        }
    },
    filters: {
        money: function (value) {
            if (!value) {
                return '';
            }

            return numeral(value).format('0,0');
        }
    },
    mounted: function() {
        var self = this;

        exchangeAccounts.getByUser()
            .then(function (response) {
                if (response && response.data && response.data.data) {
                    self.accounts = response.data.data;
                }
            })
            .catch(function (error) {
                self.errors = errorHandler.extractErrorsFromResponse(error.response);
            });
    }
}
</script>