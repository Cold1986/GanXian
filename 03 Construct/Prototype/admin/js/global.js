/* close popup */
$(".popup-bg").click(function(){
	$(this).parents(".popup").hide();
})
$(".popup-cancel").click(function(){
	$(this).parents(".popup").hide();
})

/* popup */
function clickPopup(clickObj,popupWindow,fn){
	fn=fn||function(){};
	$(clickObj).click(function(){
		$(popupWindow).show();
		fn();
	})
}

