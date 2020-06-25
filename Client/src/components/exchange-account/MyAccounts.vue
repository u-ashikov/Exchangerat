<template>
<div>
    <h1 class="text-center">My Accounts</h1>
    <hr class="mb-3"/>
    <div class="row">
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
                <a href="#" class="btn btn-primary">Transactions</a>
              </div>
            </div>
        </div>
    </div>
    </div>
</template>

<script>
import exchangeAccounts from '../../queries/exchangeAccounts.js';
import { authHeader } from '../../helpers/auth-header.js';
import errorHandler from '../../helpers/error-handler';
import numeral from 'numeral';

export default {
    data: function () {
        return {
            accounts: []
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

        exchangeAccounts.getByUser(authHeader())
        .then(function (response) {
            self.accounts = response.data.data;
        })
        .catch(function (error) {
            errorHandler.extractErrorsFromResponse(error.response);
        });
    }
}
</script>