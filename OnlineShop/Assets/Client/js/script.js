$(document).ready(function(){

    // Delete 
    $('.delete').click(function(){
        var el = this;
      
        // Delete id
        var deleteid = $(this).data('id');
        
        // Confirm box
        bootbox.confirm("Do you really want to delete record?", function(result) {
            
            if(result){
                // AJAX Request
                $.ajax({
                    url: 'ajaxfile.php',
                    type: 'POST',
                    data: { id:deleteid },
                    success: function(response){

                        // Removing row from HTML Table
                        if(response == 1){
							$(el).closest('tr').css('background','tomato');
							$(el).closest('tr').fadeOut(800,function(){
								$(this).remove();
							});
						}else{
							bootbox.alert('Record not deleted.');
						}

                    }
                });
            }
            
        });
        
    });
});