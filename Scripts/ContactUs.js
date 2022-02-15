/*Regex Explanation

(?=.*[0-9]) postive look ahead. Ensures that there is atleast one digit

[- +()0 - 9]+ matches numbers, spaces, plus sign, hyphen and brackets
*/
$.validator.addMethod("mobile10d", function (value, element) {
    return this.optional(element) || /^(?=.*[0-9])[- +()0-9]+$/.test(value);
}, "Please enter a valid 10 digit Phone No."); //Example for Valid Phone No:+(91)123-456-7890

$.validator.addMethod("isemail",function (value, element) {
        return this.optional(element) || /[a-z]+@[a-z]+\.[a-z]+/.test(value);
}, "Please enter Valid Email Address");

$.validator.addMethod("noSpace", function (value, element) {
    return value == '' || value.trim().length!=0
},"Spaces are not allowed");

$("#frmContact").validate({
    rules: {
        Contact_Name: {
            required: true,
            noSpace: true
        },
        Email_Address: {
            required: "#Phone_No:blank",
            email: true,
            noSpace: true,
			isemail:true
        },
        Phone_No: {
            required: "#Email_Address:blank",
            mobile10d: true
        },
        Account_holder: {
            required: true
        },
        Preferred_Branch: {
            required:true
        },
        Query_Type: {
            required: true
        },
        Message: {
            required: true,
            minlength: 30,
            noSpace: true
        },
        privacy_check: {
            required: true
        }

    },
    messages: {
        Contact_Name: {
            required: "Name is required"
        },
        Email_Address: {
            required: "Either Email ID or Phone No is required",
            email: "Please enter valid Email ID"
        },
        Phone_No: {
            required: "Either Email ID or Phone No is required",
            mobile10d: "Please enter valid Phone No"
        },
        Account_holder: {
            required: "Please select an option"
        },
        Preferred_Branch: {
            required: "Please select Preferred branch"
        },
        Query_Type: {
            required: "Please select Type of Query"
        },
        Message: {
            required: "Please enter message",
            minlength: "Message should be atleast 30 characters"
        },
        privacy_check: {
            required: "Please select Privacy checkbox"
        }
    },
    errorPlacement: function (error, element) {
        if (element.is(":radio")) {
            error.appendTo(element.parents(".radio-box"));
        }
        else if (element.is(":checkbox")) {
            error.appendTo(element.parents(".check"));
        }
        else {
            error.insertAfter(element);
        }
    },
    submitHandler: function (form) {
        form.submit();
    }
});
