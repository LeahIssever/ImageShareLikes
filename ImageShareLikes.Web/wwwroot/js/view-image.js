$(() => {
    console.log("hello");

    const imageId = $('#image-id').val();

    setInterval(function () {
        setImageLikes();
    }, 1000);

    $(".btn").on('click', function () {
        $.post('/home/incrementLikesForImage', { id: imageId }, function() {
            setImageLikes();  
        })
        $(this).prop('disabled', true);
    })

    function setImageLikes() {
        $.get('/home/getLikesForImage', { id: imageId }, function(likesCount) {
            $('#likes-count').text(likesCount);
        })
    }


});