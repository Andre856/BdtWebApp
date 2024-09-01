
let stripe, customer, price, card, cardNumber, cardExpiry, cardCvc, elements, startcard, style;

function initializeStripe(stripePubKey) {
    stripe = window.Stripe(stripePubKey);

    elements = stripe.elements();

    style = {
        base: {
            color: '#32325d',
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: 'antialiased',
            fontSize: '16px',
            '::placeholder': {
                color: '#aab7c4'
            }
        },
        invalid: {
            color: '#fa755a',
            iconColor: '#fa755a'
        }
    };

    startcard = true;

    if (startcard) {
        cardNumber = elements.create('cardNumber', { style: style });
        cardExpiry = elements.create('cardExpiry', { style: style });
        cardCvc = elements.create('cardCvc', { style: style });
        startcard = false;
    }

    cardNumber.mount('#cardNumber-element');
    cardExpiry.mount('#cardExpiry-element');
    cardCvc.mount('#cardCvc-element');

    cardNumber.on('change', function (event) {
        displayError(event, 'cardNumber-element-errors');
    });

    cardExpiry.on('change', function (event) {
        displayError(event, 'cardExpiry-element-errors');
    });

    cardCvc.on('change', function (event) {
        displayError(event, 'cardCvc-element-errors');
    });
}

function displayError(event, elementId) {
    let displayError = document.getElementById(elementId);

    if (event.error) {
        displayError.textContent = event.error.message;
    } else {
        displayError.textContent = '';
    }
}

function createPaymentMethod(dotnetHelper, cardNumberElement, billingemail, billingName, line1, line2, city, postalCode, country) {
    return stripe
        .createPaymentMethod({
            type: 'card',
            card: cardNumberElement,
            billing_details: {
                name: billingName,
                email: billingemail,
                address: {
                    line1: line1,
                    line2: line2,
                    city: city,
                    postal_code: postalCode,
                    country: country
                }
            }
        })
        .then((result) => {
            if (result.error) {

                displayError(result);


            } else {

                createSubscription(dotnetHelper, result.paymentMethod.id);
            }
        });
}

function createPaymentMethodServer(dotnetHelper, billingemail, billingName, line1, line2, city, postalCode, country) {
    createPaymentMethod(dotnetHelper, cardNumber, billingemail, billingName, line1, line2, city, postalCode, country);
}

function createSubscription(dotnetHelper, paymentMethodId) {
    dotnetHelper.invokeMethodAsync('Subscribe', paymentMethodId);
    dotnetHelper.dispose();
}
