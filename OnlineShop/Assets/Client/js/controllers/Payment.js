
$(function () {
	var creditly = Creditly2.initialize(
		'.creditly-wrapper .expiration-month-and-year-2',
		'.creditly-wrapper .credit-card-number-2',
		'.creditly-wrapper .security-code-2',
		'.creditly-wrapper .card-type');

	$(".creditly-card-form-2 .submit").click(function (e) {
		e.preventDefault();
		var output = creditly.validate();
		if (output) {
			// Your validated credit card output
			console.log(output);
		}
	});

	/*<!--creditcard-->*/
	var creditly = Creditly.initialize(
		'.creditly-wrapper .expiration-month-and-year',
		'.creditly-wrapper .credit-card-number',
		'.creditly-wrapper .security-code',
		'.creditly-wrapper .card-type');


	$(".creditly-card-form .submit").click(function (e) {
		e.preventDefault();
		var output = creditly.validate();
		if (output) {
			// Your validated credit card output
			console.log(output);
		}
	});

	//Horizontal Tab
	$('#parentHorizontalTab').easyResponsiveTabs({
		type: 'default', //Types: default, vertical, accordion
		width: 'auto', //auto or any width like 600px
		fit: true, // 100% fit in a container
		tabidentify: 'hor_1', // The tab groups identifier
		activate: function (event) { // Callback function if tab is switched
			var $tab = $(this);
			var $info = $('#nested-tabInfo');
			var $name = $('span', $info);
			$name.text($tab.text());
			$info.show();
		}
	});

	/*< !--popup modal(for location)-->*/
	$('.popup-with-zoom-anim').magnificPopup({
		type: 'inline',
		fixedContentPos: false,
		fixedBgPos: true,
		overflowY: 'auto',
		closeBtnInside: true,
		preloader: false,
		midClick: true,
		removalDelay: 300,
		mainClass: 'my-mfp-zoom-in'
	});

});
