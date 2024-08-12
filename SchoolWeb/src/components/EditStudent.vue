<template>
    <q-dialog ref="dialogRef" v-model="isOpen">
        <q-card class="q-dialog-plugin">
            <q-card-section>
                <strong v-if="id != null">Edit Student: {{ firstName }} {{ lastName }}</strong>
                <strong v-else>Add New Student:</strong>
            </q-card-section>
            <q-card-section>
                <q-form @submit="onSubmit" @reset="onReset" class="q-gutter-md">
                    <q-input filled v-model="firstName" label="First Name *" lazy-rules
                        :rules="[val => val && val.length > 0 || 'Please type something']" />

                    <q-input filled v-model="lastName" label="Last Name *" lazy-rules
                        :rules="[val => val && val.length > 0 || 'Please type something']" />

                    <q-input filled v-model="dateOfBirth" type="date" label="Date of Birth *" required lazy-rules
                        :rules="['Please select birth date']" />
                    <q-input filled v-model="email" label="Email" type="email" />

                    <q-input filled v-model="phoneNumber" label="Phone Number" />

                    <div>
                        <q-btn label="Submit" type="submit" color="primary" />
                        <q-btn label="Reset" type="reset" color="primary" flat class="q-ml-sm" />
                    </div>
                </q-form>
            </q-card-section>
            <q-card-section>
                <slot name="cancelBtn" @click="handleCancelClick"></slot>
                <slot name="confirmBtn" @click="handleConfirmClick"></slot>
            </q-card-section>
        </q-card>
    </q-dialog>
</template>

<script>
import { date, useQuasar } from 'quasar'
import { ref } from 'vue'
import axios from 'axios'

export default {
    props: {
        data: {
            type: Object
        }
    },
    setup(props) {
        console.log(props.data)
        const id = ref(props.data.id)
        const firstName = ref(props.data.firstName)
        const lastName = ref(props.data.lastName)
        const dateOfBirth = ref(props.data.dateOfBirth ? new Date(props.data.dateOfBirth).toISOString().slice(0, 10) : null)
        const email = ref(props.data.email)
        const phoneNumber = ref(props.data.phoneNumber)
        const isOpen = ref(true)

        return {
            id,
            firstName,
            lastName,
            dateOfBirth,
            email,
            phoneNumber,
            isOpen,

            onSubmit() {
                if (id.value == null) {
                    axios.post(import.meta.env.VITE_APP_API_URL + 'students', {
                        id: 0,
                        firstName: firstName.value,
                        lastName: lastName.value,
                        dateOfBirth: new Date(dateOfBirth.value),
                        email: email.value,
                        phoneNumber: phoneNumber.value
                    })
                        .then((res) => {
                            console.log('success')
                            console.log(res.data)
                            // props.data.id.value = res.data.id
                            // props.data.firstName.value = firstName
                            // props.data.lastName.value = lastName
                            // props.data.dateOfBirth.value = dateOfBirth
                            // props.data.email.value = email
                            // props.data.phoneNumber.value = phoneNumber
                            isOpen.value = false
                        })
                        .catch((error) => {
                            console.log('error')
                            console.log(error)
                            // error = error.data.message
                        }
                        )
                }
                else {


                    axios.put(import.meta.env.VITE_APP_API_URL + 'students/' + id.value, {
                        id: id.value,
                        firstName: firstName.value,
                        lastName: lastName.value,
                        dateOfBirth: new Date(dateOfBirth.value),
                        email: email.value,
                        phoneNumber: phoneNumber.value
                    })
                        .then((res) => {
                            console.log('success')
                            console.log(res.data)
                            // props.data.firstName.value = firstName
                            // props.data.lastName.value = lastName
                            // props.data.dateOfBirth.value = dateOfBirth
                            // props.data.email.value = email
                            // props.data.phoneNumber.value = phoneNumber
                            isOpen.value = false
                        })
                        .catch((error) => {
                            console.log('error')
                            console.log(error)
                            // error = error.data.message
                        }
                        )
                }
            },

            onReset() {
                firstName.value = null
                lastName.value = null
                dateOfBirth.value = null
                email.value = null
                phoneNumber.value = null
                // data.value = null
            }
        }
    }
}
</script>