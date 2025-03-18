import { createApp } from "https://unpkg.com/vue@3.5.13/dist/vue.esm-browser.js";

createApp({
    data() {
        return {
            isMounted: false,
            model: {
                fullName: "",
                phoneNumber: "",
                email: "",
                content: ""
            },
            errors: new Map(),
            isValidated: false
        }
    },
    computed: {
        isFullNameValid() {
            if (!this.isValidated) {
                return null;
            }

            return !this.errors.get("fullName");
        },
        fullNameInputClassName() {
            if (this.isFullNameValid == null) {
                return;
            }

            if (!this.isFullNameValid) {
                return "is-invalid";
            }

            return "is-valid";
        },
        fullNameValidationMessageClassName() {
            if (this.isFullNameValid == null) {
                return;
            }

            if (!this.isFullNameValid) {
                return "text-danger";
            }

            return "text-success";
        },
        fullNameValidationMessage() {
            return this.errors.get("fullName");
        }
    },
    mounted() {
        this.isMounted = true;
    },
    watch: {
        "model.fullName"(/** @type {string} */ fullName) {
            this.isValidated = true;
            if (!fullName) {
                this.errors.set("fullName", "Họ và tên không được để trống");
                return;
            } else if (fullName.length > 50) {
                this.errors.set("fullName", "Họ và tên chỉ được chứa tối đa 50 ký tự");
                return;
            } else {
                this.errors.delete("fullName");
            }
        }
    },
}).mount("#vue-app")