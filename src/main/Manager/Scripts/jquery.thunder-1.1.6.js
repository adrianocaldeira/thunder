(function ($) {
    $.thunder = {};

    $.thunder.settings = {
        version: '1.1.6',
        images: {
            loadingModal: '/content/jquerythunder/images/loading_modal.gif',
            loadingGrid: '/content/jquerythunder/images/loading_grid.gif',
            loadingForm: '/content/jquerythunder/images/loading_form.gif'
        },
        confirm: {
            label: { yes: 'Yes', no: 'No' }
        },
        alert: {
            label: { ok: 'OK' }
        }
    };

    var methods = {
        message: {
            create: function ($this, cssClass, message, options) {
                var settings = {
                    animate: true,
                    focus: false,
                    close: {
                        show: true,
                        title: 'Close this notification'
                    },
                    autoClose: {
                        enable: false,
                        delay: 5000
                    }
                };

                $.extend(settings, options);

                $this.addClass('thunder-notification');
                $this.hide();
                $this.empty();

                var $focus = $({});
                var $targetScroll = ($this.parent().css('overflow') == 'visible' ? $('html:not(:animated),body:not(:animated)') : $this.parent());
                var $content = $('<div class="thunder-notification-content"></div>');

                $this.addClass(cssClass);
                $this.append($content);

                if ($.isArray(message) && message.length > 0) {
                    var ul = $('<ul></ul>');
                    $.each(message, function () {
                        ul.append($('<li></li>').html(this.Text));
                    });
                    $content.html(ul);
                } else {
                    $content.html(message);
                }

                if (settings.focus) {
                    $focus = $this.prev('.thunder-notification-focus');
                    if ($focus.size() == 0) {
                        $this.before('<div class="thunder-notification-focus"></div>');
                        $focus = $this.prev('.thunder-notification-focus');
                    }
                }

                $this.css('opacity', '1');

                if (settings.animate) {
                    if (settings.focus) {
                        $targetScroll.animate({ scrollTop: $focus.offset().top - 20 }, 'slow', function () {
                            $this.slideDown();
                        });
                    } else {
                        $this.slideDown();
                    }
                } else {
                    $targetScroll.scrollTop(($focus.offset().top - 20));
                    $this.show();
                }

                if (settings.close.show) {
                    var $close = $('<a class="close" href="#"></a>');

                    $close.attr('title', settings.close.title);
                    $this.append($close);

                    $close.click(function (e) {
                        if (settings.animate) {
                            $this.slideUp();
                        } else {
                            $this.hide();
                        }
                        e.preventDefault();
                    });

                    if (settings.autoClose.enable) {
                        window.setTimeout(function () {
                            if ($this.css('display') != 'none') {
                                $close.trigger('click');
                            }
                        }, settings.autoClose.delay == undefined ? 5000 : settings.autoClose.delay);
                    }
                }
            },
            success: function (message, options) {
                return this.each(function () {
                    methods.message.create($(this), 'thunder-success', message, options);
                });
            },
            error: function (message, options) {
                return this.each(function () {
                    methods.message.create($(this), 'thunder-error', message, options);
                });
            },
            attention: function (message, options) {
                return this.each(function () {
                    methods.message.create($(this), 'thunder-attention', message, options);
                });
            },
            information: function (message, options) {
                return this.each(function () {
                    methods.message.create($(this), 'thunder-information', message, options);
                });
            }
        }
    };

    $.fn.message = function (method) {
        if (methods.message[method]) {
            return methods.message[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method == 'object' || typeof method == 'string') {
            return methods.message.success.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.message');
        }
    };

    $.fn.ajaxForm = function (options) {
        var $form = $(this);
        var settings = $.extend({
            message: null,
            loading: null,
            focus: true,
            disableElements: 'input,select,textarea,button',
            cssFieldError: 'thunder-form-field-error',
            onSuccess: function () {
            },
            onBefore: function () {
            },
            onComplete: function () {
            },
            onBeforeSubmit: function () {

            }
        }, options);
        var $message = $(settings.message);
        var $loading = $(settings.loading);
        var $button = $form.find('input[type="submit"]');
        var $targetScroll = ($form.parent().css('overflow') == 'visible' ? $('html:not(:animated),body:not(:animated)') : $form.parent());

        if ($message.size() == 0 && $form.prev('.thunder-form-message').size() == 0) {
            $form.before('<div class="thunder-form-message"></div>');
            $message = $form.prev('.thunder-form-message');
        }

        if ($loading.size() == 0) {
            $button.after('<span class="thunder-form-loading"></span>');
            $loading = $form.find('.thunder-form-loading');
            $loading.css({
                'margin-top': -$button.outerHeight(),
                'margin-left': $button.outerWidth() + 2
            });
        }

        $form.data('message', $message);

        $message.hide();
        $loading.hide();

        if (!$form.attr('action') || $.trim($form.attr('action')) == '') {
            $message.message('error', 'Form action no exist.');
        }

        $form.live('submit', function () {
            $message.hide();

            settings.onBeforeSubmit();

            $.ajax({
                statusCode: statusCode($message, { focus: true }),
                url: $form.attr('action'),
                type: $form.attr('method'),
                headers: { 'Thunder-Ajax': true },
                data: $form.serialize(),
                beforeSend: function () {
                    $(settings.disableElements, $form).disableElement();
                    $('input,select,textarea', $form).removeClass(settings.cssFieldError);
                    $loading.show();
                    settings.onBefore();
                },
                complete: function () {
                    $(settings.disableElements, $form).enableElement();
                    $loading.hide();
                    settings.onComplete();
                },
                success: function (r) {
                    window.setTimeout(function () {
                        if (typeof (r) == 'object') {
                            if (r.Status) {
                                if (r.Status == 200) {
                                    settings.onSuccess($form, r);
                                } else {
                                    if (r.Messages) {
                                        if (r.Status == 202) {
                                            $message.message('error', r.Messages, { focus: true });
                                        } else if (r.Status == 203) {
                                            $message.message('information', r.Messages, { focus: true });
                                        } else if (r.Status == 204) {
                                            $message.message('attention', r.Messages, { focus: true });
                                        }
                                        $.each(r.Messages, function () {
                                            $('input[name="' + this.Field + '"],select[name="' + this.Field + '"],textarea[name="' + this.Field + '"]').addClass(settings.cssFieldError);
                                        });
                                    } else {
                                        $message.message('error', 'Message no exist in request result.', { focus: true });
                                    }
                                }
                            }
                        } else {
                            if ($(r).is('.thunder-notification')) {
                                $message.html(r);
                                if (settings.focus) {
                                    $targetScroll.animate({ scrollTop: $message.offset().top - 20 }, 'slow', function () {
                                        $message.slideDown();
                                    });
                                } else {
                                    $message.slideDown();
                                }
                            } else {
                                settings.onSuccess($form, r);
                            }
                        }
                    }, 0);
                }
            });

            return false;
        });
    };

    $.fn.grid = function (options) {
        var $grid = $(this);
        var settings = $.extend({
            message: null,
            loading: null,
            form: null,
            url: $grid.data('url'),
            pageSize: !$grid.data('page-size') ? 10 : $grid.data('page-size'),
            load: !$grid.data('load') ? true : $grid.data('load'),
            orders: [],
            onComplete: function () {
            },
            onBeforeSubmit: function () {
            }
        }, options);
        var $message = $(settings.message);
        var $loading = $(settings.loading);
        var $form = $(settings.form);
        var $content = $('<div class="thunder-grid-content"></div>');

        if (!settings.load) {
            $content.html($grid.html());
        }

        $grid.html($content);

        if ($message.size() == 0 && $grid.prev('.thunder-grid-message').size() == 0) {
            $grid.before('<div class="thunder-grid-message"></div>');
            $message = $grid.prev('.thunder-grid-message');
        }

        if ($form.size() == 0) {
            $grid.prepend('<form class="thunder-grid-form"></form>');
            $form = $('.thunder-grid-form', $grid);
        }

        $form.append('<input type="hidden" name="CurrentPage" value="0" />');
        $form.append('<input type="hidden" name="PageSize" value="' + settings.pageSize + '" />');
        $form.setOrders(settings.orders);

        if (!settings.url || settings.url == '') {
            $message.message('error', 'Url is null.');
            return;
        }

        var createLoading = function () {
            if ($(settings.loading).size() == 0) {
                $content.html('<div class="thunder-grid-loading"><img src="' + $.thunder.settings.images.loadingGrid + '" /></div>');
                $loading = $('.thunder-grid-loading', $content);
            }

            $loading.hide();
        };

        var load = function (loading) {
            settings.onBeforeSubmit();

            $.ajax({
                statusCode: statusCode($message),
                type: 'POST',
                data: $form.serialize(),
                url: settings.url,
                headers: { 'Thunder-Ajax': true },
                beforeSend: function () {
                    loading.show();
                },
                complete: function () {
                    loading.hide();
                },
                success: function (r) {
                    if (typeof (r) == 'string') {
                        $content.html(r);
                        $('tbody tr:even', $grid).addClass('even');
                        settings.onComplete($grid);
                    }
                }
            });
        };

        var paginate = function (currentPage) {
            $('input:hidden[name="CurrentPage"]', $form).val(currentPage);
            load($('.thunder-paged-loading', $grid));
        };

        if (settings.load) {
            createLoading();
            load($loading);
        }

        $form.submit(function () {
            $('input:hidden[name="CurrentPage"]', $form).val(0);
            createLoading();
            load($loading);
            return false;
        });

        $('a.thunder-grid-paged', $grid).live('click', function (e) {
            var $this = $(this);
            if (!$this.is('.disabled')) {
                paginate($this.data('page'));
            }
            e.preventDefault();
        });

        $('a.thunder-grid-order', $grid).live('click', function (e) {
            var $this = $(this);
            if ($this.data('column') != 'undefined' && $this.data('asc') != 'undefined') {
                $form.setOrders([{ 'Column': $this.data('column'), 'Asc': $this.data('asc')}]);
                $('input:hidden[name="CurrentPage"]', $form).val(0);
                load($loading);
            }
            e.preventDefault();
        });

        $('select.thunder-grid-paged', $grid).live('change', function () {
            var $this = $(this);
            paginate($('option:selected', $this).val());
        });
    };

    $.fn.modal = function (options) {
        var settings = $.extend({
            iframe: false,
            iframeScroll: 'no',
            url: '',
            width: 600,
            centerLoading: true,
            cssClass: '',
            noCache: true,
            onOpen: function () {
            },
            onClose: function () {
            }
        }, options);

        return this.each(function () {
            var $this = $(this);

            $this.live('click', function (e) {
                $.extend(settings, {
                    iframe: ($this.data('iframe') != undefined ? $this.data('iframe') : settings.iframe),
                    iframeScroll: ($this.data('iframe-scroll') != undefined ? $this.data('iframe-scroll') : settings.iframeScroll),
                    url: ($this.is('a') ? $this.attr('href') : settings.url),
                    width: ($this.data('width') != undefined ? $this.data('width') : settings.width),
                    height: ($this.data('height') != undefined ? $this.data('height') : settings.height),
                    centerLoading: ($this.data('center-loading') != undefined ? $this.data('center-loading') : settings.centerLoading),
                    cssClass: ($this.data('css-class') != undefined ? $this.data('css-class') : settings.cssClass),
                    noCache: ($this.data('no-cache') != undefined ? $this.data('no-cache') : settings.noCache)
                });

                $.modal(settings);
                e.preventDefault();
            });
        });
    };

    $.modal = function (options) {
        var settings = $.extend({
            iframe: false,
            iframeScroll: 'no',
            url: '',
            width: 600,
            centerLoading: true,
            cssClass: '',
            noCache: true,
            onOpen: function () {
            },
            onClose: function () {
            }
        }, options);
        var $loading = '<div class="thunder-modal-loading"><img src="' + $.thunder.settings.images.loadingModal + '" /></div>';

        if ($('body')['dialog'] == undefined) {
            $.error('This project not implement jquery.ui.');
            return;
        }

        if ($('.thunder-modal').size() == 0) {
            $('body').prepend('<div class="thunder-modal"></div>');
        }

        var $modal = $('.thunder-modal', $('body'))
            .addClass(settings.cssClass);

        $modal.dialog($.extend(settings, {
            autoOpen: true,
            modal: true,
            resizable: false,
            draggable: false,
            open: function () {
                $('.ui-dialog-titlebar ').remove();

                $modal.empty();
                $modal.append($loading);
                $loading = $('.thunder-modal-loading', $modal);

                if (settings.centerLoading) {
                    $('img', $loading).load(function () {
                        $loading.css({
                            'position': 'absolute',
                            'top': '50%',
                            'left': '50%'
                        });

                        if ($.browser.msie) {
                            var img = this;
                            $loading.show();
                            window.setTimeout(function () {
                                $loading.css({
                                    'margin-left': '-' + (img.width / 2) + 'px',
                                    'margin-top': '-' + (img.height / 2) + 'px'
                                });
                            }, 10);
                        } else {
                            $loading.css({
                                'margin-left': '-' + (this.width / 2) + 'px',
                                'margin-top': '-' + (this.height / 2) + 'px'
                            }).show();
                        }
                    });
                } else {
                    $loading.show();
                }

                if (settings.iframe) {
                    var $iframe = $('<iframe frameborder="0" marginheight="0" marginwidth="0" scrolling="auto"></iframe>').hide();

                    if (settings.noCache) {
                        if (settings.url.lastIndexOf('?') != -1) {
                            settings.url += '&';
                        } else {
                            settings.url += '?';
                        }
                        settings.url += 'nocache=' + parseInt(Math.random() * 9999999999);
                    }

                    $iframe.attr('scrolling', settings.iframeScroll);
                    $iframe.attr('width', $modal.width());
                    $iframe.attr('height', $modal.height());

                    $modal.css('overflow', 'hidden');
                    $modal.append($iframe);

                    $iframe.attr('src', settings.url);
                    $iframe.load(function () {
                        $iframe.show();
                        $loading.remove();

                        var $close = $iframe.contents().find('.thunder-modal-close');

                        $close.click(function (e) {
                            e.preventDefault();
                            $modal.dialog('close');
                        });

                        if (settings.closeOnEscape == undefined || settings.closeOnEscape) {
                            $iframe.contents().on('keydown', function (evt) {
                                if (evt.keyCode === $.ui.keyCode.ESCAPE) {
                                    $modal.dialog('close');
                                }
                                evt.stopPropagation();
                            });
                        }

                        settings.onOpen($iframe.contents());
                    });
                } else {
                    $modal.append('<div class="thunder-modal-message"></div>');
                    $.ajax({
                        statusCode: statusCode($('.thunder-modal-message', $modal), { close: { show: false} }),
                        headers: { 'Thunder-Ajax': true, 'Thunder-Modal': true },
                        url: settings.url,
                        success: function (html) {
                            $modal.html(html);
                            $('.thunder-modal-close', $modal).click(function (e) {
                                e.preventDefault();
                                $modal.dialog('close');
                            });
                            settings.onOpen($modal);
                        }
                    });
                }
            },
            beforeClose: function () {
                settings.onClose($modal);
                $modal.empty();
                $modal.dialog('destroy');
            }
        }));
    };

    $.closeModal = function () {
        if ($('.thunder-modal').size() > 0) {
            $('.thunder-modal').dialog('close');
        }
    };

    $.confirm = function (message, options) {
        var settings = $.extend($.thunder.settings.confirm, {
            width: 480,
            height: 'auto',
            cssClass: '',
            onYes: function () {
            },
            onNo: function () {
            }
        }, options);

        if ($('body')['dialog'] == undefined) {
            $.error('This project not implement jquery.ui.');
            return;
        }

        if ($('.thunder-confirm').size() == 0) {
            $('body').prepend('<div class="thunder-confirm"></div>');
        }

        var $yes = $('<a href="#" class="thunder-confirm-yes"></a>').html(settings.label.yes);
        var $no = $('<a href="#" class="thunder-confirm-no"></a>').html(settings.label.no);
        var $message = $('<div class="thunder-confirm-message"></div>').html(message);
        var $action = $('<div class="thunder-confirm-action"></div>').append($yes).append($no);
        var $confirm = $('.thunder-confirm', $('body'))
            .addClass(settings.cssClass)
            .append($message)
            .append($action);

        $yes.click(function (e) {
            settings.onYes();
            $confirm.dialog('close');
            e.preventDefault();
        });

        $no.click(function (e) {
            settings.onNo();
            $confirm.dialog('close');
            e.preventDefault();
        });

        $confirm.dialog({
            autoOpen: true,
            modal: true,
            resizable: false,
            closeOnEscape: false,
            draggable: false,
            width: settings.width,
            height: settings.height,
            open: function () { $('.ui-dialog-titlebar ').remove(); },
            close: function () {
                $confirm.remove();
                $confirm.dialog('destroy');
            }
        });
    };

    $.alert = function (message, options) {
        var settings = $.extend($.thunder.settings.alert, {
            width: 480,
            height: 'auto',
            cssClass: '',
            onOk: function () {
            }
        }, options);

        if ($('body')['dialog'] == undefined) {
            $.error('This project not implement jquery.ui.');
            return;
        }

        if ($('.thunder-alert').size() == 0) {
            $('body').prepend('<div class="thunder-alert"></div>');
        }

        var $ok = $('<a href="#" class="thunder-alert-ok"></a>').html(settings.label.ok);
        var $message = $('<div class="thunder-alert-message"></div>').html(message);
        var $action = $('<div class="thunder-alert-action"></div>').append($ok);
        var $alert = $('.thunder-alert', $('body'))
            .addClass(settings.cssClass)
            .append($message)
            .append($action);

        $ok.click(function (e) {
            settings.onOk();
            $alert.dialog('close');
            e.preventDefault();
        });

        $alert.dialog({
            autoOpen: true,
            modal: true,
            resizable: false,
            closeOnEscape: false,
            draggable: false,
            width: settings.width,
            height: settings.height,
            open: function () { $('.ui-dialog-titlebar ').remove(); },
            close: function () {
                $alert.remove();
                $alert.dialog('destroy');
            }
        });
    };

    $.fn.setOrders = function (orders) {
        return this.each(function () {
            var $this = $(this);
            $('input.thunder-grid-order', $this).remove();
            $.each(orders, function (i) {
                $this.append('<input type="hidden" class="thunder-grid-order" name="Orders[' + i + '].Column" value="' + this.Column + '" />');
                $this.append('<input type="hidden" class="thunder-grid-order" name="Orders[' + i + '].Asc" value="' + this.Asc + '" />');
            });
        });
    };

    $.fn.disableElement = function () {
        return this.each(function () {
            $(this).attr('disabled', 'disabled');
        });
    };

    $.fn.enableElement = function () {
        return this.each(function () {
            $(this).removeAttr('disabled');
        });
    };

    $.fn.serializeObject = function () {
        var data = {};
        var array = this.serializeArray();
        $.each(array, function () {
            if (data[this.name]) {
                if (!data[this.name].push) {
                    data[this.name] = [data[this.name]];
                }
                data[this.name].push(this.value || '');
            } else {
                data[this.name] = this.value || '';
            }
        });
        return data;
    };

    statusCode = function ($message, options) {
        var settings = $.extend({}, options);

        return {
            400: function () {
                $message.message('error', 'Bad request.', settings);
                $('.thunder-modal-loading').hide();
            },
            401: function () {
                $message.message('error', 'Unauthorized.', settings);
                $('.thunder-modal-loading').hide();
            },
            403: function () {
                $message.message('error', 'Forbidden.', settings);
                $('.thunder-modal-loading').hide();
            },
            404: function () {
                $message.message('error', 'Page not found.', settings);
                $('.thunder-modal-loading').hide();
            },
            405: function () {
                $message.message('error', 'Method not allowed.', settings);
                $('.thunder-modal-loading').hide();
            },
            407: function () {
                $message.message('error', 'Proxy authentication required.', settings);
                $('.thunder-modal-loading').hide();
            },
            408: function () {
                $message.message('error', 'Request timeout.', settings);
                $('.thunder-modal-loading').hide();
            },
            500: function () {
                $message.message('error', 'Internal server error.', settings);
                $('.thunder-modal-loading').hide();
            },
            501: function () {
                $message.message('error', 'Not implemented.', settings);
                $('.thunder-modal-loading').hide();
            },
            502: function () {
                $message.message('error', 'Bad gateway.', settings);
                $('.thunder-modal-loading').hide();
            },
            503: function () {
                $message.message('error', 'Service unavailable.', settings);
                $('.thunder-modal-loading').hide();
            }
        };
    };

    String.prototype.replaceAll = function (searchValue, replaceValue) {
        var str = this;
        var position = str.indexOf(searchValue);
        while (position > -1) {
            str = str.replace(searchValue, replaceValue);
            position = str.indexOf(searchValue);
        }
        return (str);
    };

    $(function () {
        $.each($('form[data-ajax]'), function () {
            var $form = $(this);
            if ($form.data('ajax')) {
                $form.ajaxForm({
                    onBefore: function () {
                        if (window[$form.data('ajax-before')]) {
                            window[$form.data('ajax-before')]();
                        }
                    },
                    onComplete: function () {
                        if (window[$form.data('ajax-complete')]) {
                            window[$form.data('ajax-complete')]();
                        }
                    },
                    onSuccess: function (form, r) {
                        if (window[$form.data('ajax-success')]) {
                            window[$form.data('ajax-success')](form, r);
                        }
                    }
                });
            }
        });
    });

})(jQuery);
