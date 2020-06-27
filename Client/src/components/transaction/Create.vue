<template>
    <div>
        <h1 class="text-center dispay-4 mt-3 mb-5">Create Transaction</h1>
        <form method="post" class="col-4 mx-auto" v-on:submit.prevent="sendTransaction">
            <div class="form-group">
                <label class="h6" for="sender-account">Sender Account</label>
                <input type="text" name="sender-account" id="sender-account" class="form-control" v-model.lazy="senderAccount" v-bind:class="{ invalid: $v.senderAccount.$error }" v-on:blur="$v.senderAccount.$touch()" />
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
            </div>

            <div class="form-group">
                <label class="h6" for="description">Description</label>
                <textarea name="description" id="description" cols="10" rows="4" class="form-control" v-model.lazy="description" v-bind:class="{ invalid: $v.description.$error }" v-on:blur="$v.description.$touch()"></textarea>
                <p class="text-danger" v-if="$v.description.$error && !$v.description.required">The Description field is required.</p>
            </div>

            <input type="submit" value="Send" class="btn btn-success">
        </form>
    </div>
</template>

<script>
import { validations } from '../../validations/transactions/create'

export default {
    data: function () {
        return {
            senderAccount: '',
            receiverAccount: '',
            amount: 0,
            description: ''
        }
    },
    validations: validations,
    methods: {
        sendTransaction: function () {
            alert('Send!');
        }
    }
}
</script>